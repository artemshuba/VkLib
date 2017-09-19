using System;
using Newtonsoft.Json.Linq;
using VkLib.Core.Video;
using VkLib.Extensions;

namespace VkLib.Core.Attachments
{
    public class VkVideoAttachment : VkAttachment
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Photo 130
        /// </summary>
        public string Photo130 { get; set; }

        /// <summary>
        /// Photo 320
        /// </summary>
        public string Photo320 { get; set; }

        /// <summary>
        /// Photo 800
        /// </summary>
        public string Photo800 { get; set; }

        /// <summary>
        /// Views count
        /// </summary>
        public long Views { get; set; }

        /// <summary>
        /// Comments count
        /// </summary>
        public long Comments { get; set; }

        /// <summary>
        /// Date added
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public override string Type { get { return "video"; } }

        public VkVideoAttachment()
        {

        }

        public VkVideoAttachment(VkVideo video)
        {
            Id = video.Id;
            OwnerId = video.OwnerId;
            Photo130 = video.Photo130;
            Photo320 = video.Photo320;
            Photo800 = video.Photo640;
            Title = video.Title;
            Description = video.Description;
        }

        public static new VkVideoAttachment FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkVideoAttachment();

            result.Id = (long)json["id"];

            if (json["owner_id"] != null)
                result.OwnerId = (long)json["owner_id"];

            if (json["title"] != null)
                result.Title = (string)json["title"];

            if (json["description"] != null)
                result.Description = (string)json["description"];

            if (json["duration"] != null)
                result.Duration = TimeSpan.FromSeconds((long)json["duration"]);

            if (json["photo_130"] != null)
                result.Photo130 = (string)json["photo_130"];

            if (json["photo_320"] != null)
                result.Photo320 = (string)json["photo_320"];

            if (json["photo_800"] != null)
                result.Photo800 = (string)json["photo_800"];

            if (json["views"] != null)
                result.Views = (long)json["views"];

            if (json["comments"] != null)
                result.Comments = (long)json["comments"];

            if (json["date"] != null)
                result.Date = DateTimeExtensions.UnixTimeStampToDateTime((double)json["date"]);

            if (json["access_key"] != null)
                result.AccessKey = (string)json["access_key"];

            return result;
        }
    }
}