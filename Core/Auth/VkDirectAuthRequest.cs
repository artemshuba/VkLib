using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLib.Auth;
using VkLib.Error;
using VkLib.Extensions;

namespace VkLib.Core.Auth
{
    public class VkDirectAuthRequest
    {
        private readonly Vk _vkontakte;

        internal VkDirectAuthRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        /// <summary>
        /// <para>Direct auth with login and password.</para>
        /// <para>See also: <seealso cref="http://vk.com/pages?oid=-1&p=Прямая_авторизация"/></para>
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <param name="scopeSettings">Scope settings</param>
        /// <param name="captchaSid">Captcha sid</param>
        /// <param name="captchaKey">Captcha key</param>
        /// <returns><see cref="VkAccessToken"/></returns>
        public async Task<VkAccessToken> Login(string login, string password, VkScopeSettings scopeSettings = VkScopeSettings.CanAccessFriends,
            string captchaSid = null, string captchaKey = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"username", login},
                {"password", password},
                {"grant_type", "password"},
                {"scope", ((int) scopeSettings).ToString(CultureInfo.InvariantCulture)}
            };

            if (!string.IsNullOrEmpty(captchaSid) && !string.IsNullOrEmpty(captchaKey))
            {
                parameters.Add("captcha_sid", captchaSid);
                parameters.Add("captcha_key", captchaKey);
            }

            parameters.Add("client_id", _vkontakte.AppId);
            parameters.Add("client_secret", _vkontakte.ClientSecret);

            var response = await VkRequest.GetAsync(VkConst.DirectAuthUrl, parameters);

            var token = new VkAccessToken();
            token.Token = response["access_token"].Value<string>();
            token.UserId = response["user_id"].Value<long>();
            token.ExpiresIn = response["expires_in"].Value<long>() == 0 ? DateTime.MaxValue : DateTimeExtensions.UnixTimeStampToDateTime(response["expires_in"].Value<long>());
            _vkontakte.AccessToken = token;
            return token;
        }
    }
}
