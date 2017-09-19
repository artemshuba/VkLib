using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Attachments
{
    public class VkGiftAttachment : VkAttachment
    {
        public string Thumb256 { get; set; }

        public string Thumb96 { get; set; }

        public string Thumb48 { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public override string Type { get { return "gift"; } }

        public static new VkGiftAttachment FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkGiftAttachment();

            result.Id = (long)json["id"];

            if (json["thumb_256"] != null)
                result.Thumb256 = (string)json["thumb_256"];

            if (json["thumb_96"] != null)
                result.Thumb96 = (string)json["thumb_96"];

            if (json["thumb_48"] != null)
                result.Thumb48 = (string)json["thumb_48"];

            return result;
        }
    }
}