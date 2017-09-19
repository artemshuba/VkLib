using System;
using Newtonsoft.Json.Linq;
using VkLib.Extensions;

namespace VkLib.Core.Attachments
{
    public class VkDocumentAttachment : VkAttachment
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Size in bytes
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Extension
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Add date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Document type
        /// </summary>
        public int DocumentType { get; set; }

        /// <summary>
        /// Image 100x75 (if it's image)
        /// </summary>
        public string Photo100 { get; set; }

        /// <summary>
        /// Image 130x100 (if it's image)
        /// </summary>
        public string Photo130 { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public override string Type { get { return "doc"; } }

        public static new VkDocumentAttachment FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkDocumentAttachment();

            result.Id = (long)json["id"];
            result.OwnerId = (long)json["owner_id"];

            if (json["title"] != null)
                result.Title = (string)json["title"];

            if (json["size"] != null)
                result.Size = long.Parse((string)json["size"]);

            if (json["ext"] != null)
                result.Ext = (string)json["ext"];

            if (json["url"] != null)
                result.Url = (string)json["url"];

            if (json["photo_100"] != null)
                result.Photo100 = (string)json["photo_100"];

            if (json["photo_130"] != null)
                result.Photo130 = (string)json["photo_130"];

            if (json["date"] != null)
                result.Date = DateTimeExtensions.UnixTimeStampToDateTime((double)json["date"]);

            if (json["type"] != null)
                result.DocumentType = (int)json["type"];

            return result;
        }
    }
}