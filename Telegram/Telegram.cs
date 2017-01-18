using System;
using System.Collections.Generic;

namespace Telegram
{
    /// <summary>
    /// The main Telegram class. This should exist only once per bot you want to use. </summary>
    /// <remarks>
    /// Keep in mind that this class is blocking and therefore does not work too good
    /// with big loops and intensly used bots. This will be solved in future updates. </remarks>
    public class Telegram
    {
        // Fields
        private string apiToken = "";
        private int start_time = 0;
        private bool ready = false;
        private int last_update = 0;

        private const string url = "https://api.telegram.org/bot{0}/{1}";

        // Delegates
        public delegate void ReceiveMessageDel(Message message);
        public delegate void ReceiveFileDel(File file, Message message);

        // Events
        private event ReceiveMessageDel receiveEvent;
        private event ReceiveMessageDel receiveCommandEvent;
        private event ReceiveFileDel receiveFileEvent;
        
        /// <summary>
        /// Telegram constructor
        /// </summary>
        /// <param name="token">Your Bot API Token.</param>
        /// <param name="time">UTC time, used to ignore messages send before then./</param>
        /// <remarks>
        /// Keep in mind that messages of bots only exist for 24 hours and are removed
        /// after 'getUpdates' has been called with an offset!</remarks>
        public Telegram(string token, int time = 0)
        {
            apiToken = token; 
            start_time = (time > 0) ? (time) : ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

            ready = (bool)getMe()["ok"] == true;
        }

        /// <summary>
        /// getter for API token 
        /// </summary>
        public string APIToken { get { return apiToken; } }

        /// <summary>
        /// Set the event that handles normal messages.
        /// </summary>
        public void SetReceiveEvent(ReceiveMessageDel del) { receiveEvent += del; }

        /// <summary>
        /// Set the event that handles command messages.
        /// </summary>
        public void SetReceiveCommandEvent(ReceiveMessageDel del) { receiveCommandEvent += del; }

        /// <summary>
        /// Set the event that handles files.
        /// </summary>
        public void SetReceiveFileEvent(ReceiveFileDel del) { receiveFileEvent += del; }

        /// <summary>
        /// Used to see if the bot is successfully initialized. If not try checking the API Token.
        /// </summary>
        /// <returns>True when the bot is successfully initialized. </returns>
        public bool isReady()
        {
            if (!ready)
                return (ready = (bool)getMe()["ok"] == true);
            else return true;
        }

        /// <summary>
        /// Used for the updating cycle.
        /// </summary>
        public void Update()
        {
            if (!isReady()) return;

            Queue<Update> queue = new Queue<Update>();
            List<Update> updates = getUpdates(last_update);

            if (updates != null)
                foreach (Update u in updates) // Add all the new messages to the queue to be processed.
                {
                    queue.Enqueue(u);
                    last_update = u.update_id;
                }
            
            if (receiveEvent != null || receiveCommandEvent != null)
                while (queue.Size > 0)
                {
                    Message tmp = queue.Peek().message;

                    if (tmp.isFile())
                        if (receiveFileEvent != null) receiveFileEvent((File)tmp.file, queue.Dequeue().message);
                        else receiveEvent(queue.Dequeue().message);
                    else if (tmp.isCommand())
                        if (receiveCommandEvent != null) receiveCommandEvent(queue.Dequeue().message);
                        else receiveEvent(queue.Dequeue().message);
                    else receiveEvent(queue.Dequeue().message);
                }
        }

        /// <summary>
        /// Gets the result of the 'getMe' method.
        /// See 'https://core.telegram.org/bots/api#getme' for more information.
        /// </summary>
        private JSON getMe()
        {
            Request r = new Request(string.Format(url, apiToken, "getMe"));
            r.Exec();

            JSON json = new JSON(r.Data);
            json.Parse();
            return json;
        }

        /// <summary>
        /// Gets the result of the 'getUpdates' method.
        /// See 'https://core.telegram.org/bots/api#getupdates' for more information.
        /// </summary>
        /// <param name="id">Offset as described on the API Documentation.</param>
        private List<Update> getUpdates(int id = 0)
        {
            if (!isReady()) return null;

            string urlx = url;
            if (id != 0) urlx += "?offset=" + id+1;
            Request r = new Request(string.Format(urlx, apiToken, "getUpdates"));

            if (!r.Exec())
            {
                // Failed to do a webrequest. Try without an offset next iteration.
                last_update = 0;
                return null;
            }

            // Start parsing the incomming data.
            JSON json = new JSON(r.Data);
            json.Parse();

            if (json.isError() || json["result"] == null) return null;

            // Add all updates from a JSONObject into a List.
            List<Update> updates = new List<Update>();
            foreach (JSONObject o in ((JSONArray)json["result"]).Data)
                if (((id > 0 && (int)o["update_id"] > id) || id == 0) && (int)(o["message.date"]) >= start_time)
                    updates.Add(new Update(o));

            return updates;
        }
        
        /// <summary>
        /// Sends a message.
        /// See 'https://core.telegram.org/bots/api#sendmessage' for more information.
        /// </summary>
        /// <param name="message">Message to send.</param>
        public Message sendMessage(Message message)
        {
            string urlx = url + "?chat_id={2}&text={3}";
            Request r = new Request(string.Format(urlx, apiToken, "sendMessage", message.chat.id, message.text));
            r.Exec();

            return message;
        }

        /// <summary>
        /// Sends a message.
        /// See 'https://core.telegram.org/bots/api#sendmessage' for more information.
        /// </summary>
        /// <param name="chat_id">The chat id to send a message to.</param>
        /// <param name="message">The text to send.</param>
        public Message sendMessage(int chat_id, string message)
        {
            string urlx = url + "?chat_id={2}&text={3}";
            Request r = new Request(string.Format(urlx, apiToken, "sendMessage", chat_id, message));
            r.Exec();

            return new Message() {
                chat = new Chat() { id = chat_id },
                text = message,
                date = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
            };
        }
    }
}
