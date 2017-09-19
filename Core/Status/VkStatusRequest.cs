using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Status
{
    public class VkStatusRequest
    {
        private readonly Vk _vkontakte;

        internal VkStatusRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        /// <summary>
        /// Устанавливает статус пользователя
        /// </summary>
        /// <param name="text">Текст статуса</param>
        /// <param name="audioId">ID аудиозаписи для музыкального статуса</param>
        /// <param name="audioOwnerId">ID владельца аудиозаписи</param>
        /// <returns></returns>
        public async Task<bool> Set(string text, string audioId, string audioOwnerId)
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(text))
                parameters.Add("text", text);

            if (!string.IsNullOrEmpty(audioId) && !string.IsNullOrEmpty(audioOwnerId))
                parameters.Add("audio", audioOwnerId + "_" + audioId);

            parameters.Add("access_token", _vkontakte.AccessToken.Token);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "status.set", parameters);

            return response["response"]?.Value<int>() == 1;
        }

        public async Task<string> Get(string uid)
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(uid))
                parameters.Add("uid", uid);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "status.get", parameters);

            return response["response"]?.Value<string>();
        }
    }
}