using System.IO;
using System.Net;

namespace Telegram
{
    public class File
    {
        // This class is as defined in the Telegram Bot API found at:
        // https://core.telegram.org/bots/api#file

        public string file_id = "";

        // Optional
        public int file_size = 0;
        public string file_path = "";

        protected const string url = "https://api.telegram.org/bot{0}/{1}";
        protected const string file_url = "https://api.telegram.org/file/bot{0}/{1}";

        public File() { }
        public File(JSONObject json)
        {
            if (json == null) return;

            file_id = (string)json["file_id"];
            file_size = (int)json["file_size"];
            file_path = (string)json["file_path"];
        }

        public virtual void Dowload(string apiToken, string fileURI = null)
        {
            using (WebClient c = new WebClient())
            {
                if (file_path == "") file_path = GetFilePath(apiToken, file_id);

                string saveURI = fileURI ?? Directory.GetCurrentDirectory() + "\\" + file_path.Replace("/", "\\");
                
                Directory.CreateDirectory(saveURI.Substring(0, saveURI.LastIndexOf("\\")));
                c.DownloadFile(string.Format(file_url, apiToken, file_path), saveURI);
            }
        }

        protected string GetFilePath(string apiToken, string file_id)
        {
            string urlx = url + "?file_id={2}";
            Request r = new Request(string.Format(urlx, apiToken, "getFile", file_id));
            r.Exec();

            JSON json = new JSON(r.Data);
            json.Parse();

            if ((bool)json["ok"])
                return new File((JSONObject)json["result"]).file_path;
            return "";
        }
    }
}
