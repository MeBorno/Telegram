using System;
using System.Threading;
using System.Collections.Generic;
using Telegram;

namespace TelegramSample
{
    class Program
    {
        // This is a simple example of how to use the Telegram API.

        public static List<Message> msgs = new List<Message>();
        public static Telegram.Telegram t;

        static void Main(string[] args)
        {
            // Init Telegram object and set events.
            t = new Telegram.Telegram("164266520:AAF2Kc7XL7P5iuRXs5rjgJ1dwwIHHW7N-0w");
            t.SetReceiveEvent(OnMessageReceive);
            t.SetReceiveCommandEvent(OnCommandReveive);
            t.SetReceiveFileEvent(OnFileReceive);


            while (true)
            {
                t.Update(); // Fetch new messages.

                // Display information to screen.
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Messages recieved: {0}", msgs.Count);
                Console.WriteLine("Last message ID: {0}", (msgs.Count == 0)? 0 : msgs[msgs.Count - 1]?.message_id);
                Console.SetCursorPosition(0, 2);

                // Display the latstest 10 (possible) messages.
                for (int i = (msgs.Count > 20) ? msgs.Count - 10 : 0;
                         i < msgs.Count; i++)
                    Console.WriteLine(msgs[i].from.first_name + "> " + msgs[i].text);
                
                // Sleep for some time just to give the API some time.
                Thread.Sleep(500);   
            }
        }

        // Just a regular message.
        public static void OnMessageReceive(Message m)
        {
            msgs.Add(m);
        }

        // A message starting with a '/'.
        public static void OnCommandReveive(Message m)
        {
            msgs.Add(m);

            if (m.text.StartsWith("/start"))
            {
                t.sendMessage(m.chat.id, "Oh hello there! "+EmojiFaces.SmileBlush+"\nMy name is Rumble and I'll be helping you out a bit.\n\nI know some commands for you to use, why not try one out already? Just type in '/help' to get some more info.");
            }
            else if (m.text.StartsWith("/help"))
            {
                t.sendMessage(m.chat.id, "HELP?! Whats wrong?\nHere are some commands you can use:\n/smile,/kiss");
            }
            else if (m.text.StartsWith("/smile"))
            {
                t.sendMessage(m.chat.id, "Ok! I'll smile for you!");
                t.sendMessage(m.chat.id, EmojiFaces.Smiling);
            }
            else if (m.text.StartsWith("/kiss"))
            {
                t.sendMessage(m.chat.id, "1000 KISSES FOR YOU!");
                string message = "";
                for (int i = 0; i < 1000; i++)
                    message += "\U0001F618";
                t.sendMessage(m.chat.id, message);
            }
        }

        // A message that contains a File.
        public static void OnFileReceive(File file, Message message)
        {
            if (message.isPhoto())
            {
                message.text = "{photo}";
                file.Dowload(t.APIToken);
            }

            msgs.Add(message);
        }
    }

    
}
