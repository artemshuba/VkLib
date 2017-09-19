using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Video
{
    public class VkUploadVideoResponse
    {
        public string VideoHash { get; set; }

        public long VideoId { get; set; }

        public long Size { get; set; }

        public static VkUploadVideoResponse FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            var result = new VkUploadVideoResponse();

            if (json["video_hash"] != null)
                result.VideoHash = (string)json["video_hash"];

            if (json["size"] != null)
                result.Size = (long)json["size"];

            if (json["video_id"] != null)
                result.VideoId = (long)json["video_id"];

            return result;
        }
    }
}
