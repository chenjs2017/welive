using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Plugin.Connectivity;
using System.Net.Http.Headers;
using System.IO;
namespace WeLive
{
    public class PropertyDataStore : BaseDataStore
    {
        public async Task<Property> GetItem(string postID)
        {
			var arr = await GetItemsAsync(postID);
            if (arr.Count > 0)
                return arr[0];
            throw new Exception("信息不存在(information not exits)"); 
        }
        public async Task<List<Property>> GetItemsAsync(string postID)
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception(ErrorMessage.NotLogin);
			List<Property> items = new List<Property>();
            string url = $"api/properties/get_my_properties/?count=50";
            if (!string.IsNullOrEmpty(postID))
            {
                url += "&post_id=" + postID;
            }
            var json = await client.GetStringAsync(url);
            ErrorMessage.CheckRespond(json);

            JObject root = JObject.Parse(json);
            var values = root["posts"].Children();

            foreach (JToken result in values)
            {
                try
                {
                    Property item = result.ToObject<Property>();
                    items.Add(item);
                }
                catch (System.Exception ex)
                {
                    Debug.Write(ex.Message);
                    throw new Exception(ErrorMessage.DecodeEorror) ;
                }
            }
            return items;
        }

        public async Task<string> AddItemAsync(Property item)
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception (ErrorMessage.NotLogin);
            
   
            var serializedItem = JsonConvert.SerializeObject(item);
            var content = new StringContent(serializedItem, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/properties/create_property/", content);
            var stringContent = await response.Content.ReadAsStringAsync();
            ErrorMessage.CheckRespond(stringContent);
            return stringContent.Trim();
     
        }

        public async Task DeleteItemAsync(string id)
        {
            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception(ErrorMessage.NotLogin);

            string url = $"api/properties/del/?post_id=" + id;

            var json = await client.GetStringAsync(url);
            ErrorMessage.CheckRespond(json);
        }
    }
}
