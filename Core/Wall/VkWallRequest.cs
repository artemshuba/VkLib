using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLib.Core.Attachments;
using VkLib.Core.Groups;
using VkLib.Core.Users;

namespace VkLib.Core.Wall
{
    public class VkWallRequest
    {
        private readonly Vk _vkontakte;

        internal VkWallRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task<long> Post(long ownerId = 0, string message = null, IEnumerable<VkAttachment> attachments = null, bool fromGroup = false, bool signed = false)
        {
            var parameters = new Dictionary<string, string>();

            if (ownerId != 0)
                parameters.Add("owner_id", ownerId.ToString());

            if (!string.IsNullOrEmpty(message))
                parameters.Add("message", message);

            if (attachments != null)
            {
                parameters.Add("attachments", string.Join(",", attachments));
            }

            if (fromGroup)
                parameters.Add("from_group", "1");

            if (signed)
                parameters.Add("signed", "1");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "wall.post", parameters);

            if (response["response"] != null)
                return (long)response["response"]["post_id"];
            return 0;
        }

        public async Task<VkItemsResponse<VkWallEntry>> Get(long ownerId, string filter, int count = 0, int offset = 0)
        {
            var parameters = new Dictionary<string, string>();

            if (ownerId != 0)
                parameters.Add("owner_id", ownerId.ToString());

            if (!string.IsNullOrEmpty(filter))
                parameters.Add("filter", filter);

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            parameters.Add("extended", "1");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "wall.get", parameters);

            if (response.SelectToken("response.items") != null)
            {
                var result = new VkItemsResponse<VkWallEntry>();
                result.Items = (from p in response["response"]["items"] where p.HasValues select VkWallEntry.FromJson(p)).ToList();
                result.TotalCount = response["response"]["count"].Value<int>();

                if (response["response"]["profiles"] != null)
                {
                    var users = (from n in response["response"]["profiles"] select VkProfile.FromJson(n)).ToList();
                    foreach (var entry in result.Items)
                    {
                        entry.Author = users.FirstOrDefault(u => u.Id == entry.SourceId);
                    }
                }

                if (response["response"]["groups"] != null)
                {
                    var groups = (from n in response["response"]["groups"] select VkGroup.FromJson(n)).ToList();
                    foreach (var entry in result.Items.Where(e => e.Author == null))
                    {
                        entry.Author = groups.FirstOrDefault(g => g.Id == Math.Abs(entry.SourceId));
                    }
                }

                return result;
            }

            return null;
        }
    }
}
