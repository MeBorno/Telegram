using System.Net;
using System.Collections.Generic;
using System.IO;

namespace Telegram
{
    public class Photo : File
    {
        // This class is an abstraction made for ease-of-use.
        // It combines the functionality of a File with the "photo"-data used in Message.
        // * It automatically selects the highest resolution possible.
        // For more info check:
        // https://core.telegram.org/bots/api#message and
        // https://core.telegram.org/bots/api#photosize

        private List<PhotoSize> sizes;

        public int width = 0;
        public int height = 0;

        public Photo() { }
        public Photo(JSONArray json)
        {
            sizes = new List<PhotoSize>();

            foreach (object o in json.Data)
                sizes.Add(new PhotoSize((JSONObject)o));

            PhotoSize highestRes = null;
            foreach (PhotoSize ps in sizes)
                if (ps.file_size > (highestRes?.file_size ?? 0)) highestRes = ps;

            file_id = highestRes.file_id;
            file_size = highestRes.file_size;
            width = highestRes.width;
            height = highestRes.height;
        }
    }
}
