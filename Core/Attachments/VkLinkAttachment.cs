using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Attachments
{
    public class VkLinkAttachment : VkAttachment
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Url;
        }

        /// <summary>
        /// Type
        /// </summary>
        public override string Type { get { return "link"; } }

        public new static VkLinkAttachment FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkLinkAttachment();

            if (json["url"] != null)
                result.Url = (string)json["url"];

            if (json["title"] != null)
                result.Title = (string)json["title"];

            if (json["description"] != null)
                result.Description = (string)json["description"];

            return result;
        }
    }
}