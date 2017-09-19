using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Messages
{
    public class VkDialog
    {
        public int Unread { get; set; }

        public VkMessage Message { get; set; }

        internal static VkDialog FromJson(JToken json)
        {
            if (json == null)
                throw new Exception("Json can't be null");

            var result = new VkDialog();
            if (json["unread"] != null)
            result.Unread = (int)json["unread"];
            if (json["message"] != null)
                result.Message = VkMessage.FromJson(json["message"]);

            return result;
        }
    }
}
