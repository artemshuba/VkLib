using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Messages
{
    public class VkPlace
    {
        public string Title { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        internal static VkPlace FromJson(JToken json)
        {
            if (json == null)
                throw new Exception("Json can't be null");

            var result = new VkPlace();
            result.Title = (string)json["title"];
            result.Country = (string)json["country"];
            result.City = (string)json["city"];

            return result;
        }
    }
}