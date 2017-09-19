using System;
using Newtonsoft.Json.Linq;
using VkLib.Extensions;

namespace VkLib.Core.Photos
{
    public class VkPhoto
    {
        public long Id { get; set; }

        public long AlbumId { get; set; }

        public long OwnerId { get; set; }

        /// <summary>
        /// 75x75
        /// </summary>
        public string Photo75 { get; set; }

        /// <summary>
        /// 130x130
        /// </summary>
        public string Photo130 { get; set; }

        /// <summary>
        /// 604x604
        /// </summary>
        public string Photo604 { get; set; }

        /// <summary>
        /// 807x807
        /// </summary>
        public string Photo807 { get; set; }

        /// <summary>
        /// 1280x1024
        /// </summary>
        public string Photo1280 { get; set; }

        /// <summary>
        /// 2560x2048
        /// </summary>
        public string Photo2560 { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public static VkPhoto FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            var result = new VkPhoto();
            if (json["id"] != null)
                result.Id = (long)json["id"];

            if (json["album_id"] != null)
                result.AlbumId = (long)json["album_id"];

            if (json["owner_id"] != null)
                result.OwnerId = (long)json["owner_id"];

            if (json["photo_75"] != null)
                result.Photo75 = (string)json["photo_75"];

            if (json["photo_130"] != null)
                result.Photo130 = (string)json["photo_130"];

            if (json["photo_604"] != null)
                result.Photo604 = (string)json["photo_604"];

            if (json["photo_807"] != null)
                result.Photo807 = (string)json["photo_807"];

            if (json["photo_1280"] != null)
                result.Photo1280 = (string)json["photo_1280"];

            if (json["photo_2560"] != null)
                result.Photo2560 = (string)json["photo_2560"];

            if (json["width"] != null)
                result.Width = (int)json["width"];

            if (json["height"] != null)
                result.Height = (int)json["height"];

            if (json["text"] != null)
                result.Text = (string)json["text"];

            if (json["date"] != null)
                result.Created = DateTimeExtensions.UnixTimeStampToDateTime((long)json["date"]);

            return result;
        }
    }
}
