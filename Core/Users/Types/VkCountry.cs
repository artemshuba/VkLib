using Newtonsoft.Json.Linq;

namespace VkLib.Core.Users.Types
{
    /// <summary>
    /// Country
    /// </summary>
    public class VkCountry
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        internal static VkCountry FromJson(JToken json)
        {
            var result = new VkCountry();

            result.Id = json["id"].Value<int>();
            result.Title = json["title"].Value<string>();

            return result;
        }
    }
}
