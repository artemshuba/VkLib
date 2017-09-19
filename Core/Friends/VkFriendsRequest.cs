using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkLib.Core.Users;

namespace VkLib.Core.Friends
{
    public enum FriendsOrder
    {
        ByName,
        ByRating,
        Random
    }

    public class VkFriendsRequest
    {
        private readonly Vk _vkontakte;

        internal VkFriendsRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task<VkItemsResponse<VkProfile>> Get(long userId, string fields, string nameCase, int count, int offset, FriendsOrder order = FriendsOrder.ByName)
        {
            var parameters = new Dictionary<string, string>();

            if (userId > 0)
                parameters.Add("user_id", userId.ToString());

            if (!string.IsNullOrWhiteSpace(fields))
                parameters.Add("fields", fields);

            if (!string.IsNullOrWhiteSpace(nameCase))
                parameters.Add("name_case", nameCase);

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            switch (order)
            {
                case FriendsOrder.ByName:
                    parameters.Add("order", "name");
                    break;

                case FriendsOrder.ByRating:
                    parameters.Add("order", "hints");
                    break;

                case FriendsOrder.Random:
                    parameters.Add("order", "random");
                    break;
            }


            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "friends.get", parameters);
            
            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkProfile>(response["response"]["items"].Select(VkProfile.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkProfile>.Empty;
        }
    }
}
