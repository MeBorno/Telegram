using System.Net;
using System.IO;

namespace Telegram
{
    /// <summary>
    /// Request class turns a webrequest stream into a string.
    /// </summary>
    public class Request
    {
        // Fields
        private string url = "";
        private string data = "";

        public Request(string url)
        {
            this.url = url;
        }
        
        public string Data { get { return data; } }

        /// <summary>
        /// Executes the request with the given url.
        /// </summary>
        /// <returns>True if the request executed normally. False if anything went wrong.</returns>
        public bool Exec()
        {
            WebRequest req = WebRequest.Create(url);
            req.Timeout = 500;
            req.Proxy = null;

            try
            {
                WebResponse response = req.GetResponse();
                data = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                response.Close();
                return true;
            } catch (WebException e) { return false; }
        }
    }
}
