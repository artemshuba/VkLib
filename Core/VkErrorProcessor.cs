using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using VkLib.Error;

namespace VkLib.Core
{
    internal static class VkErrorProcessor
    {
        public static bool ProcessError(JObject response)
        {
            if (response["error"] != null)
            {
                if (response.SelectToken("error.error_code") != null)
                {
                    var errorCode = response.SelectToken("error.error_code").Value<string>();
                    switch (errorCode)
                    {
                        case "6":
                            throw new VkTooManyRequestsException();
                        case "201":
                            throw new VkAccessDeniedException();
                        case "221":
                            throw new VkStatusBroadcastDisabledException();
                        case "5":
                            throw new VkInvalidTokenException();
                        case "9":
                            throw new VkFloodControlException();
                        case "14":
                            throw new VkCaptchaNeededException(response["error"]["captcha_sid"].Value<string>(), response["error"]["captcha_img"].Value<string>());
                    }
                }

                if (response["error"].HasValues)
                {
                    Debug.WriteLine(response["error"]["error_code"].Value<string>() + ":" + response["error"]["error_msg"].Value<string>());
                    switch (response["error"]["error_code"].Value<int>())
                    {
                        case 15:
                        case 7:
                            throw new VkAccessDeniedException();

                        case 17:
                            throw new VkNeedValidationException() { RedirectUri = new Uri(response["error"]["redirect_uri"].Value<string>()) };

                        case 9:
                            throw new VkFloodControlException();

                        default:
                            var s = response["error"]["error_code"].Value<string>() + ":" +
                                    response["error"]["error_msg"].Value<string>();
                            throw new VkException(s, s);
                    }
                }
                else
                    switch (response["error"].Value<string>())
                    {
                        case "need_captcha":
                            throw new VkCaptchaNeededException(response["captcha_sid"].Value<string>(), response["captcha_img"].Value<string>());
                        case "need_validation":
                            throw new VkNeedValidationException() { RedirectUri = new Uri(response["redirect_uri"].Value<string>()) };
                        case "invalid_client":
                            throw new VkInvalidClientException();
                        default:
                            throw new VkException(response["error"].Value<string>(), response["error"].Value<string>());
                    }
            }
            return false;
        }
    }
}
