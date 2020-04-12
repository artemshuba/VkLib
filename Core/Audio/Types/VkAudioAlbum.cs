using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Audio
{
    public class VkAudioAlbum
    {
        public long OwnerId { get; set; }

        public long Id { get; set; }

        public string Title { get; set; }

        public string AccessKey { get; set; }

        public VkThumb Thumb { get; set; }

        public static VkAudioAlbum FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkAudioAlbum();

            result.Id = json["id"].Value<long>();
            result.OwnerId = Math.Abs(json["owner_id"].Value<long>());
            result.Title = WebUtility.HtmlDecode(json["title"].Value<string>());
            result.AccessKey = json["access_key"].Value<string>();
            if (json["thumb"] != null)
                result.Thumb = json["thumb"].ToObject<VkThumb>();
            return result;
        }
    }
}
