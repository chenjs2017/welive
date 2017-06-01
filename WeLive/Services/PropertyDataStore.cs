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

        public async Task<IEnumerable<Property>> GetItemsAsync()
        {

            if (!CrossConnectivity.Current.IsConnected)
                throw new Exception(ErrorMessage.NotLogin);
			List<Property> items = new List<Property>();

			var json = await client.GetStringAsync($"api/properties/get_my_properties/?count=50");
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
