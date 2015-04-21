using Newtonsoft.Json.Linq;

namespace VkLib.Core.Users.Types
{
    /// <summary>
    /// City
    /// </summary>
    public class VkCity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        internal static VkCity FromJson(JToken json)
        {
            var result = new VkCity();

            result.Id = json["id"].Value<int>();
            result.Title = json["title"].Value<string>();

            return result;
        }
    }
}
