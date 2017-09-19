using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Video
{
    public class VkVideoSaveResponse
    {
        public string UploadUrl { get; set; }

        public long VideoId { get; set; }

        public long OwnerId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string AccessKey { get; set; }

        public static VkVideoSaveResponse FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            var result = new VkVideoSaveResponse();
            result.UploadUrl = (string)json["upload_url"];

            if (json["access_key"] != null)
                result.AccessKey = (string)json["access_key"];

            if (json["description"] != null)
                result.Description = (string)json["description"];

            if (json["owner_id"] != null)
                result.OwnerId = (long)json["owner_id"];

            if (json["title"] != null)
                result.Title = (string)json["title"];

            if (json["video_id"] != null)
                result.VideoId = (long)json["video_id"];

            return result;
        }
    }
}
