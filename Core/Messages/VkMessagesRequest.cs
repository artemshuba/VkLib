using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkLib.Core.Attachments;
using VkLib.Core.Users;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Messages
{
    /// <summary>
    /// Messages request
    /// </summary>
    public class VkMessagesRequest
    {
        private readonly Vk _vkontakte;

        internal VkMessagesRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        /// <summary>
        /// <para>Send message</para>
        /// <para>See also: <seealso cref="http://vk.com/dev/messages.send"/></para>
        /// </summary>
        /// <returns>Id of new message</returns>
        public async Task<long> Send(long userId, long chatId, string message, List<VkAttachment> attachments = null, List<VkMessage> forwardMessages = null, int stickerId = 0)
        {
            var parameters = new Dictionary<string, string>();

            if (userId != 0)
                parameters.Add("user_id", userId.ToString());
            else
            {
                if (chatId != 0)
                    parameters.Add("chat_id", chatId.ToString());
                else
                    throw new Exception("User id or chat id must be specified.");
            }

            if (!string.IsNullOrEmpty(message))
                parameters.Add("message", message);

            if (attachments != null)
            {
                var a = attachments.Select(x => $"{x.Type}{x.OwnerId}_{x.Id}").ToList();

                parameters.Add("attachment", string.Join(",", a));
            }

            if (forwardMessages != null)
            {
                var m = forwardMessages.Select(x => x.Id).ToList();

                parameters.Add("forward_messages", string.Join(",", m));
            }

            if (stickerId != 0)
                parameters.Add("sticker_id", stickerId.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.send", parameters);

            return response["response"].Value<long>();
        }

        public async Task<bool> Delete(List<long> messageIds)
        {
            var parametres = new Dictionary<string, string>();

            parametres.Add("message_ids", string.Join(",", messageIds));

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.delete", parametres);

            return response["response"].Value<bool>();
        }

        public async Task<VkItemsResponse<VkDialog>> GetDialogs(int offset = 0, int count = 0, uint previewLength = 0,
            string userId = null)
        {
            var parameters = new Dictionary<string, string>();

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (previewLength > 0)
                parameters.Add("preview_length", previewLength.ToString());

            if (!string.IsNullOrEmpty(userId))
                parameters.Add("user_id", userId);

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.getDialogs", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkDialog>(response["response"]["items"].Select(VkDialog.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkDialog>.Empty;
        }

        public async Task<VkItemsResponse<VkMessage>> GetHistory(long userId, long chatId = 0, int offset = 0, int count = 0, bool rev = false)
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

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.getHistory", parametres);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkMessage>(response["response"]["items"].Select(i => VkMessage.FromJson(i, _vkontakte.ApiVersion)).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkMessage>.Empty;
        }

        public async Task<List<VkProfile>> GetChatUsers(long chatId, IEnumerable<long> chatIds = null, string fields = null, string nameCase = null)
        {
            var parametres = new Dictionary<string, string>();

            if (chatId != 0)
                parametres.Add("chat_id", chatId.ToString());
            else
            {
                if (chatIds != null)
                    parametres.Add("chat_ids", string.Join(",", chatIds));
                else
                    throw new Exception("Chat id or chat ids must be specified.");
            }

            if (!string.IsNullOrEmpty(fields))
                parametres.Add("fields", fields);

            if (!string.IsNullOrEmpty(nameCase))
                parametres.Add("name_case", nameCase);

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.getChatUsers", parametres);

            return response["response"]?.Select(VkProfile.FromJson).ToList();
        }

        public async Task<VkConversation> GetChat(long chatId, IEnumerable<long> chatIds = null, string fields = null, string nameCase = null)
        {
            if (_vkontakte.AccessToken == null || string.IsNullOrEmpty(_vkontakte.AccessToken.Token) || _vkontakte.AccessToken.HasExpired)
                throw new Exception("Access token is not valid.");

            var parametres = new Dictionary<string, string>();

            if (chatId != 0)
                parametres.Add("chat_id", chatId.ToString());
            else
            {
                if (chatIds != null)
                    parametres.Add("chat_ids", string.Join(",", chatIds));
                else
                    throw new Exception("Chat id or chat ids must be specified.");
            }

            if (!string.IsNullOrEmpty(fields))
                parametres.Add("fields", fields);

            if (!string.IsNullOrEmpty(nameCase))
                parametres.Add("name_case", nameCase);

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.getChat", parametres);

            if (response["response"] != null)
            {
                return VkConversation.FromJson(response["response"]);
            }

            return null;
        }

        public async Task<VkMessage> GetById(long messageId, int previewLength = 0)
        {
            var result = await GetById(new List<long>() { messageId }, previewLength);
            return result.Items?.FirstOrDefault();
        }

        public async Task<VkItemsResponse<VkMessage>> GetById(IEnumerable<long> messageIds, int previewLength = 0)
        {
            var parametres = new Dictionary<string, string>();

            if (messageIds != null)
                parametres.Add("message_ids", string.Join(",", messageIds));

            if (previewLength > 0)
                parametres.Add("preview_length", previewLength.ToString());

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.getById", parametres);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkMessage>(response["response"]["items"].Select(i => VkMessage.FromJson(i, _vkontakte.ApiVersion)).ToList(),
                    (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkMessage>.Empty;
        }

        public async Task<bool> MarkAsRead(IEnumerable<long> messageIds, string peerId = null, long startMessageId = -1)
        {
            var parametres = new Dictionary<string, string>();

            if (messageIds != null)
                parametres.Add("message_ids", string.Join(",", messageIds));

            if (peerId != null)
                parametres.Add("peer_id", peerId);

            if (startMessageId != -1)
                parametres.Add("start_message_id", startMessageId.ToString());

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.markAsRead", parametres);

            if (response["response"] != null && response["response"].Value<long>() == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> SetActivity(string userId = null, string type = "typing", string peerId = null)
        {
            var parametres = new Dictionary<string, string>();

            if (userId != null)
                parametres.Add("user_id", userId);

            if (peerId != null)
                parametres.Add("peer_id", peerId);

            if (type != null)
                parametres.Add("type", type);

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.setActivity", parametres);

            if (response["response"]?.Value<long>() == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<List<VkProfile>> SearchDialogs(string q, int count = 0, string fields = null)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("q", q);

            if (count > 0)
                parameters.Add("limit", count.ToString());

            if (!string.IsNullOrEmpty(fields))
                parameters.Add("fields", fields);

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.searchDialogs", parameters);

            if (response["response"] != null)
            {
                return new List<VkProfile>(response["response"].Select(VkProfile.FromJson));
            }

            return new List<VkProfile>();
        }
    }
}
