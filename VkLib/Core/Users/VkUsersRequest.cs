using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkLib.Error;

namespace VkLib.Core.Users
{
    public class VkUsersRequest
    {
        private readonly Vkontakte _vkontakte;

        internal VkUsersRequest(Vkontakte vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task<VkProfile> Get(long userId, string fields = null, string nameCase = null)
        {
            var users = await Get(new List<string>() { userId.ToString() }, fields, nameCase);
            if (users.Items != null && users.Items.Count > 0)
                return users.Items.First();

            return null;
        }

        public async Task<VkItemsResponse<VkProfile>> Get(IEnumerable<string> userIds, string fields = null, string nameCase = null)
        {
            if (_vkontakte.AccessToken == null || string.IsNullOrEmpty(_vkontakte.AccessToken.Token) || _vkontakte.AccessToken.HasExpired)
                throw new VkInvalidTokenException();

            var parameters = new Dictionary<string, string>();

            if (userIds != null)
                parameters.Add("user_ids", string.Join(",", userIds));

            if (!string.IsNullOrWhiteSpace(fields))
                parameters.Add("fields", fields);

            if (!string.IsNullOrWhiteSpace(nameCase))
                parameters.Add("name_case", nameCase);

            _vkontakte.SignMethod(parameters);

            var response = await new VkRequest(new Uri(VkConst.MethodBase + "users.get"), parameters).Execute();

            VkErrorProcessor.ProcessError(response);

            if (response.SelectToken("response") != null)
            {
                return new VkItemsResponse<VkProfile>((from u in response["response"] select VkProfile.FromJson(u)).ToList());
            }

            return VkItemsResponse<VkProfile>.Empty;
        }

        /// <summary>
        /// Returns a list of users matching the search criteria.
        /// </summary>
        /// <param name="query">Search query string</param>
        /// <param name="sortType">Sort order</param>
        /// <param name="fields">Profile fields to return. http://vk.com/dev/fields </param>
        /// <param name="city">City ID</param>
        /// <param name="country">Country ID</param>
        /// <param name="hometown">City name</param>
        /// <param name="universityCountry">ID of the country where the user graduated</param>
        /// <param name="university">ID of the institution of higher education</param>
        /// <param name="universityYear">Year of graduation from an institution of higher education</param>
        /// <param name="universityFaculty"></param>
        /// <param name="universityChair"></param>
        /// <param name="sex">Sex</param>
        /// <param name="status">Relationship status</param>
        /// <param name="ageFrom">Minimum age</param>
        /// <param name="ageTo">Maximum age</param>
        /// <param name="birthDay">Day of birth</param>
        /// <param name="birthMonth">Month of birth</param>
        /// <param name="birthYear">Year of birth</param>
        /// <param name="onlineOnly">Online only</param>
        /// <param name="withPhotoOnly">With photo only</param>
        /// <param name="schoolCountry">ID of the country where users finished school</param>
        /// <param name="schoolCity">ID of the city where users finished school</param>
        /// <param name="schoolClass">School class</param>
        /// <param name="school">ID of the school</param>
        /// <param name="schoolYear">School graduation year</param>
        /// <param name="religion">User's religious affiliation</param>
        /// <param name="interests">User's interests</param>
        /// <param name="company">Name of the company where users work</param>
        /// <param name="position">Job position</param>
        /// <param name="groupId">ID of a community to search in communities</param>
        /// <param name="fromList"></param>
        /// <param name="offset">Offset (more than 1000 ignores)</param>
        /// <param name="count">Count (maximum 1000)</param>
        /// <returns></returns>
        public async Task<VkItemsResponse<VkProfile>> Search(
            string query, VkUsersSortType sortType = VkUsersSortType.ByPopularity, string fields = null,
            int city = 0, int country = 0, string hometown = null, int universityCountry = 0, int university = 0,
            int universityYear = 0, int universityFaculty = 0, int universityChair = 0, VkUserSex sex = VkUserSex.Any,
            VkUserStatus? status = null, int ageFrom = 0, int ageTo = 0, int birthDay = 0, int birthMonth = 0, int birthYear = 0,
            bool onlineOnly = false, bool withPhotoOnly = false, int schoolCountry = 0, int schoolCity = 0, int schoolClass = 0,
            int school = 0, int schoolYear = 0, string religion = null, string interests = null, string company = null, string position = null,
            int groupId = 0, string fromList = null, int offset = 0, int count = 0)
        {
            if (_vkontakte.AccessToken == null || string.IsNullOrEmpty(_vkontakte.AccessToken.Token) || _vkontakte.AccessToken.HasExpired)
                throw new VkInvalidTokenException();

            var parameters = new Dictionary<string, string>();

            parameters.Add("q", query);
            parameters.Add("sort", ((int)VkUsersSortType.ByPopularity).ToString());

            if (!string.IsNullOrEmpty(fields))
                parameters.Add("fields", fields);

            if (city > 0)
                parameters.Add("city", city.ToString());

            if (country > 0)
                parameters.Add("country", country.ToString());

            if (!string.IsNullOrEmpty(hometown))
                parameters.Add("hometown", hometown);

            if (universityCountry > 0)
                parameters.Add("university_country", universityCountry.ToString());

            if (university > 0)
                parameters.Add("university", university.ToString());

            if (universityYear > 0)
                parameters.Add("university_year", universityYear.ToString());

            if (universityFaculty > 0)
                parameters.Add("university_faculty", universityFaculty.ToString());

            if (universityChair > 0)
                parameters.Add("university_chair", universityChair.ToString());

            parameters.Add("sex", ((int)sex).ToString());
            if (status != null)
                parameters.Add("status", ((int)status.Value).ToString());

            if (ageFrom > 0)
                parameters.Add("age_from", ageFrom.ToString());

            if (ageTo > 0)
                parameters.Add("age_to", ageTo.ToString());

            if (birthDay > 0)
                parameters.Add("birth_day", birthDay.ToString());

            if (birthMonth > 0)
                parameters.Add("birth_month", birthMonth.ToString());

            if (birthYear > 0)
                parameters.Add("birth_year", birthYear.ToString());

            if (onlineOnly)
                parameters.Add("online", "1");

            if (withPhotoOnly)
                parameters.Add("has_photo", "1");

            if (schoolCountry > 0)
                parameters.Add("school_country", schoolCountry.ToString());

            if (schoolCity > 0)
                parameters.Add("school_city", schoolCity.ToString());

            if (schoolClass > 0)
                parameters.Add("school_class", schoolClass.ToString());

            if (school > 0)
                parameters.Add("school", school.ToString());

            if (schoolYear > 0)
                parameters.Add("school_year", schoolYear.ToString());

            if (!string.IsNullOrEmpty(religion))
                parameters.Add("religion", religion);

            if (!string.IsNullOrEmpty(interests))
                parameters.Add("interests", interests);

            if (!string.IsNullOrEmpty(company))
                parameters.Add("company", company);

            if (!string.IsNullOrEmpty(position))
                parameters.Add("position", position);

            if (groupId != 0)
                parameters.Add("group_id", groupId.ToString());

            if (!string.IsNullOrEmpty(fromList))
                parameters.Add("from_list", fromList);

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await new VkRequest(new Uri(VkConst.MethodBase + "users.search"), parameters).Execute();

            VkErrorProcessor.ProcessError(response);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkProfile>(response["response"]["items"].Select(VkProfile.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkProfile>.Empty;
        }
    }
}