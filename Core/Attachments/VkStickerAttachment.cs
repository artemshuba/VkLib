using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Attachments
{
    public class VkStickerAttachment : VkAttachment
    {
        /// <summary>
        /// Id of stickers set
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 64x64 image
        /// </summary>
        public string Photo64 { get; set; }

        /// <summary>
        /// 128x128 image
        /// </summary>
        public string Photo128 { get; set; }

        /// <summary>
        /// 256x256 image
        /// </summary>
        public string Photo256 { get; set; }

        /// <summary>
        /// 352x252 image
        /// </summary>
        public string Photo352 { get; set; }

        /// <summary>
        /// Image width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Image height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public override string Type { get { return "sticker"; } }

        public static new VkStickerAttachment FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkStickerAttachment();

            result.Id = (long)json["id"];

            if (json["product_id"] != null)
                result.ProductId = (long)json["product_id"];

            if (json["photo_64"] != null)
                result.Photo64 = (string)json["photo_64"];

            if (json["photo_128"] != null)
                result.Photo128 = (string)json["photo_128"];

            if (json["photo_256"] != null)
                result.Photo256 = (string)json["photo_256"];

            if (json["photo_352"] != null)
                result.Photo352 = (string)json["photo_352"];

            if (json["width"] != null)
                result.Width = (int)json["width"];

            if (json["height"] != null)
                result.Height = (int)json["height"];

            return result;
        }
    }
}
