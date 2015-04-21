using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VkLib.Core.Users.Types;
using VkLib.Extensions;

namespace VkLib.Core.Users
{
    /// <summary>
    /// User profile
    /// <seealso cref="http://vk.com/dev/fields"/>
    /// </summary>
    public class VkProfile : VkProfileBase
    {
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public override string Name
        {
            get { return FirstName + " " + LastName; }
        }

        /// <summary>
        /// Sex
        /// </summary>
        public VkUserSex Sex { get; set; }

        /// <summary>
        /// Is online
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Is online from mobile device
        /// </summary>
        public bool IsOnlineMobile { get; set; }

        /// <summary>
        /// Last seen
        /// </summary>
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Is user verified
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// User's universities
        /// </summary>
        public List<VkUniversity> Universities { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public VkCountry Country { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public VkCity City { get; set; }

        /// <summary>
        /// Is user a friend of current user
        /// </summary>
        public bool IsFriend { get; set; }

        internal static VkProfile FromJson(JToken json)
        {
            var result = new VkProfile();

            result.Id = (long)json["id"];
            result.FirstName = (string)json["first_name"];
            result.LastName = (string)json["last_name"];

            if (json["photo"] != null) //NOTE in some methods this used instead of photo_xx
                result.Photo = (string)json["photo"];

            if (json["photo_50"] != null)
                result.Photo = (string)json["photo_50"];

            if (json["photo_100"] != null)
                result.PhotoMedium = json["photo_100"].Value<string>();

            if (json["photo_200_orig"] != null)
                result.PhotoBig = json["photo_200_orig"].Value<string>();

            if (json["photo_200"] != null)
                result.PhotoBigSquare = json["photo_200"].Value<string>();

            if (json["photo_400_orig"] != null)
                result.PhotoLarge = json["photo_400_orig"].Value<string>();

            if (json["photo_max"] != null)
                result.PhotoMaxSquare = json["photo_max"].Value<string>();

            if (json["photo_max_orig"] != null)
                result.PhotoMax = json["photo_max_orig"].Value<string>();

            if (json["online"] != null)
                result.IsOnline = (int)json["online"] == 1;

            if (json["online_mobile"] != null)
                result.IsOnlineMobile = (int)json["online"] == 1;

            if (json["last_seen"] != null)
                result.LastSeen = DateTimeExtensions.UnixTimeStampToDateTime((long)json["last_seen"]["time"]);

            if (json["sex"] != null)
                result.Sex = (VkUserSex)(int)json["sex"];

            if (json["verified"] != null)
                result.IsVerified = json["verified"].Value<int>() == 1;

            if (json["universities"] != null)
                result.Universities = json["universities"].Select(VkUniversity.FromJson).ToList();

            if (json["city"] != null)
                result.City = VkCity.FromJson(json["city"]);

            if (json["country"] != null)
                result.Country = VkCountry.FromJson(json["country"]);

            if (json["is_friend"] != null)
                result.IsFriend = json["is_friend"].Value<int>() == 1;

            return result;
        }
    }
}
