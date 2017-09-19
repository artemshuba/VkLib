using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using VkLib.Core.Users;

namespace VkLib.Core.Messages
{
    public class VkConversation
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public long AdminId { get; set; }

        public List<VkProfile> Users { get; set; }

        public static VkConversation FromJson(JToken json, string apiVersion = null)
        {
            if (json == null)
                throw new Exception("Json can't be null");

            var result = new VkConversation();
            if (json["id"] != null)
                result.Id = (long)json["id"];

            if (json["title"] != null)
                result.Title = (string)json["title"];

            if (json["admin_id"] != null)
                result.AdminId = (long)json["admin_id"];

            if (json["users"] != null)
            {
                result.Users = (from u in json["users"] select VkProfile.FromJson(u)).ToList();
            }

            return result;
        }
    }
}