using Newtonsoft.Json.Linq;
using System;

namespace VkLib.Core.Audio
{
    public class VkExtendedPlaylist
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public VkPlaylist Playlist { get; set; }

        internal static VkExtendedPlaylist FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkExtendedPlaylist();

            result.Title = json["title"].Value<string>();
            if (json["subtitle"] != null)
                result.Subtitle = json["subtitle"].Value<string>();

            result.Playlist = VkPlaylist.FromJson(json["playlist"]);

            return result;
        }
    }
}
