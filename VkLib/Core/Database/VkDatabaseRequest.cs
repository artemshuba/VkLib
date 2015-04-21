using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkLib.Core.Users;
using VkLib.Core.Users.Types;

namespace VkLib.Core.Database
{
    public class VkDatabaseRequest
    {
        private readonly Vkontakte _vkontakte;

        internal VkDatabaseRequest(Vkontakte vkontakte)
        {
            _vkontakte = vkontakte;
        }

        /// <summary>
        /// Returns a list of countries
        /// </summary>
        /// <param name="needAll">
        /// 1 — to return a full list of all countries 
        /// 0 — to return a list of countries near the current user's country (default) 
        /// </param>
        /// <param name="code">Country codes in ISO 3166-1 alpha-2 standard</param>
        /// <param name="count">Offset needed to return a specific subset of countries</param>
        /// <param name="offset">Number of countries to return</param>
        /// <returns></returns>
        public async Task<VkItemsResponse<VkCountry>> GetCountries(bool needAll = false, string code = null, int count = 0, int offset = 0)
        {
            var parameters = new Dictionary<string, string>();

            if (needAll)
                parameters.Add("need_all", "1");

            if (!string.IsNullOrWhiteSpace(code))
                parameters.Add("code", code);

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await new VkRequest(new Uri(VkConst.MethodBase + "database.getCountries"), parameters).Execute();

            VkErrorProcessor.ProcessError(response);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkCountry>(response["response"]["items"].Select(VkCountry.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkCountry>.Empty;
        }

        /// <summary>
        /// Returns a list of cities
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <param name="regionId">Region id</param>
        /// <param name="query">Search query</param>
        /// <param name="needAll">True - return all cities in the country, False - return only major cities in the country</param>
        /// <param name="count">Count</param>
        /// <param name="offset">Offset</param>
        /// <returns></returns>
        public async Task<VkItemsResponse<VkCity>> GetCities(int countryId, int regionId = 0, string query = null, bool needAll = false, int count = 0, int offset = 0)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("country_id", countryId.ToString());

            if (regionId != 0)
                parameters.Add("region_id", regionId.ToString());

            if (query != null)
                parameters.Add("q", query);

            if (needAll)
                parameters.Add("need_all", "1");

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await new VkRequest(new Uri(VkConst.MethodBase + "database.getCities"), parameters).Execute();

            VkErrorProcessor.ProcessError(response);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkCity>(response["response"]["items"].Select(VkCity.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkCity>.Empty;
        }
    }
}
