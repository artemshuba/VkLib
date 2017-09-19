using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkLib.Core.Groups;
using VkLib.Core.Users;
using VkLib.Core.Wall;

namespace VkLib.Core.Favorites
{
    public class VkFavoritesRequest
    {
        private readonly Vk _vkontakte;

        internal VkFavoritesRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task<VkItemsResponse<VkWallEntry>> GetPosts(int count = 0, int offset = 0, bool extended = true)
        {
            var parameters = new Dictionary<string, string>();

            if (count > 0)
                parameters.Add("count", count.ToString());

            if (offset > 0)
                parameters.Add("offset", offset.ToString());

            if (extended)
                parameters.Add("extended", "1");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "fave.getPosts", parameters);

            if (response.SelectToken("response.items") != null)
            {
                var result = new VkItemsResponse<VkWallEntry>((from n in response["response"]["items"] select VkWallEntry.FromJson(n)).ToList());

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
