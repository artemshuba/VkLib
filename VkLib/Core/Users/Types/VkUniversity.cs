using Newtonsoft.Json.Linq;
using VkLib.Extensions;

namespace VkLib.Core.Users
{
    /// <summary>
    /// University info in user profile
    /// </summary>
    public class VkUniversity
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Country id
        /// </summary>
        public long Country { get; set; }

        /// <summary>
        /// City id
        /// </summary>
        public long City { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Faculty id
        /// </summary>
        public long Faculty { get; set; }

        /// <summary>
        /// Faculty name
        /// </summary>
        public string FacultyName { get; set; }

        /// <summary>
        /// Chair id
        /// </summary>
        public long Chair { get; set; }

        /// <summary>
        /// Chair name
        /// </summary>
        public string ChairName { get; set; }

        /// <summary>
        /// Graduation year
        /// </summary>
        public int Graduation { get; set; }

        /// <summary>
        /// Education form
        /// </summary>
        public string EducationForm { get; set; }

        /// <summary>
        /// Education status
        /// </summary>
        public string EducationStatus { get; set; }

        internal static VkUniversity FromJson(JToken json)
        {
            var result = new VkUniversity();

            result.Id = json["id"].Value<long>();
            result.Country = json["country"].Value<long>();
            result.City = json["city"].Value<long>();
            result.Name = json["name"].Value<string>();

            if (json["faculty"] != null)
                result.Faculty = json["faculty"].Value<long>();

            if (json["faculty_name"] != null)
                result.FacultyName = json["faculty_name"].Value<string>();

            if (json["chair"] != null)
                result.Chair = json["chair"].Value<long>();

            if (json["chair_name"] != null)
                result.ChairName = json["chair_name"].Value<string>();

            if (json["graduation"] != null)
                result.Graduation = json["graduation"].Value<int>();

            if (json["education_form"] != null)
                result.EducationForm = json["education_form"].Value<string>();

            if (json["education_status"] != null)
                result.EducationStatus = json["education_status"].Value<string>();

            return result;
        }
    }
}