using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkLib.Core.Store
{
    public class VkStoreRequest
    {
        private readonly Vk _vk;

        internal VkStoreRequest(Vk vk)
        {
            _vk = vk;
        }

        public async Task<VkItemsResponse<VkStoreProduct>> GetProducts(string type, string filters, bool extended = true)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("type", type);

            parameters.Add("filters", filters);

            if (extended)
                parameters.Add("extended", "1");

            _vk.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "store.getProducts", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkStoreProduct>(response["response"]["items"].Select(VkStoreProduct.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return null;
        }
    }
}