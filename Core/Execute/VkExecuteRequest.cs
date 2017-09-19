using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkLib.Core.Messages;
using VkLib.Core.Store;
using VkLib.Core.Users;

namespace VkLib.Core.Execute
{
    public class VkExecuteRequest
    {
        private readonly Vk _vkontakte;

        public VkExecuteRequest(Vk vk)
        {
            _vkontakte = vk;
        }

        public async Task<VkItemsResponse<VkMessage>> GetChatHistoryAndMarkAsRead(long userId, long chatId = 0, int offset = 0, int count = 0, bool rev = false, bool markAsRead = false)
        {
            var parametres = new Dictionary<string, string>();

            if (userId != 0)
                parametres.Add("user_id", userId.ToString());
            else
            {
                if (chatId != 0)
                    parametres.Add("chat_id", chatId.ToString());
                else
                    throw new Exception("User id or chat id must be specified.");
            }

            if (offset > 0)
                parametres.Add("offset", offset.ToString());

            if (count > 0)
                parametres.Add("count", count.ToString());

            if (rev)
                parametres.Add("rev", "1");

            if (markAsRead)
                parametres.Add("markAsRead", "1");

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "execute.getChatHistoryAndMarkAsRead", parametres);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkMessage>(response["response"]["items"].Select(i => VkMessage.FromJson(i, _vkontakte.ApiVersion)).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkMessage>.Empty;
        }

        public async Task<List<VkProfile>> GetUsersAndGroupsById(IEnumerable<string> userIds, IEnumerable<string> groupIds = null, string fields = null, string nameCase = null)
        {
            var parameters = new Dictionary<string, string>();

            if (userIds != null)
                parameters.Add("user_ids", string.Join(",", userIds));

            if (groupIds != null)
                parameters.Add("group_ids", string.Join(",", groupIds));

            if (!string.IsNullOrWhiteSpace(fields))
                parameters.Add("fields", fields);

            if (!string.IsNullOrWhiteSpace(nameCase))
                parameters.Add("name_case", nameCase);

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "execute.getUsersAndGroupsById", parameters);

            if (response["response"] != null)
            {
                var result = new List<VkProfile>();
                var usersToken = response.SelectToken("response.users");
                if (usersToken != null)
                {
                    result.AddRange((from u in usersToken select VkProfile.FromJson(u)).ToList());
                }

                var gruopsToken = response.SelectToken("response.groups");
                if (gruopsToken != null)
                {
                    result.AddRange((from u in gruopsToken select VkProfile.FromJson(u)).ToList());
                }

                return result;
            }

            return new List<VkProfile>();
        }

        public async Task<List<VkProfile>> SearchHints(string q, int count = 9, string filters = null, bool searchGlobal = false, string fields = null)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("q", q);

            if (count != 0)
                parameters.Add("limit", count.ToString());

            if (!string.IsNullOrEmpty(filters))
                parameters.Add("filters", filters);

            parameters.Add("search_global", searchGlobal ? "1" : "0");

            if (!string.IsNullOrWhiteSpace(fields))
                parameters.Add("fields", fields);

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "execute.searchHints", parameters);

            if (response["response"] != null)
            {
                return new List<VkProfile>(from u in response["response"] select VkProfile.FromJson(u));
            }

            return new List<VkProfile>();
        }

        public async Task<VkStickerPackProduct> GetRecentStickers()
        {
            var parameters = new Dictionary<string, string>();
            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "execute.getRecentStickers", parameters);

            if (response["response"] != null)
            {
                return VkStickerPackProduct.FromJson(response["response"]);
            }

            return null;
        }


        //this method used by vk to mark access token as "true" (obtained from official app)
        public async Task GetBaseData(Dictionary<string, string> parameters)
        {
            if (_vkontakte.AccessToken == null || string.IsNullOrEmpty(_vkontakte.AccessToken.Token) || _vkontakte.AccessToken.HasExpired)
                throw new Exception("Access token is not valid.");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "execute.getBaseData", parameters);
        }
    }
}