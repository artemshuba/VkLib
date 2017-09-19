using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Groups
{
    public enum VkGroupSearchType
    {
        /// <summary>
        /// Сортировать по количеству пользователей
        /// </summary>
        ByUsers = 0,

        /// <summary>
        /// Сортировать по скорости роста
        /// </summary>
        ByGrowSpeed = 1,

        /// <summary>
        /// Сортировать по отношению дневной посещаемости ко количеству пользователей
        /// </summary>
        ByVisitsPerDay = 2,

        /// <summary>
        /// Сортировать по отношению количества лайков к количеству пользователей
        /// </summary>
        ByLikes = 3,

        /// <summary>
        /// Сортировать по отношению количества комментариев к количеству пользователей
        /// </summary>
        ByComments = 4,

        /// <summary>
        /// Сортировать по отношению количества записей в обсуждениях к количеству пользователей
        /// </summary>
        ByDiscussions = 5
    }

    public class VkGroupsRequest
    {
        private readonly Vk _vkontakte;

        internal VkGroupsRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task<VkItemsResponse<VkGroup>> Get(long userId, string fields, string filter, int count, int offset, bool extended = true)
        {
            var parameters = new Dictionary<string, string>();

            if (userId != 0)
                parameters.Add("user_id", userId.ToString());

            if (!string.IsNullOrWhiteSpace(fields))
                parameters.Add("fields", fields);

            if (!string.IsNullOrWhiteSpace(filter))
                parameters.Add("filter", filter);

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            if (extended)
                parameters.Add("extended", "1");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "groups.get", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkGroup>(response["response"]["items"].Select(VkGroup.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkGroup>.Empty;
        }

        public async Task<VkItemsResponse<VkGroup>> Search(string query, VkGroupSearchType sort = VkGroupSearchType.ByUsers, int count = 0, int offset = 0)
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(query))
                parameters.Add("q", query);

            parameters.Add("sort", ((int)sort).ToString());

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "groups.search", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkGroup>((from g in response["response"]["items"] where g.HasValues select VkGroup.FromJson(g)).ToList(), response["response"]["count"].Value<int>());
            }

            return VkItemsResponse<VkGroup>.Empty;
        }
    }
}
