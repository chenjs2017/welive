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
        
        List<Property> items;

       
        public PropertyDataStore()
        {
			
			items = new List<Property>();
        }

       

        public async Task<IEnumerable<Property>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/properties/get_my_properties/");
                JObject root = JObject.Parse(json);
                var values = root["posts"].Children();
                items.Clear(); 
                foreach (JToken result in values)
                {
					Property item = result.ToObject<Property>();
                    items.Add(item);
                }
            }

            return items;
        }

        public async Task<Property> GetItemAsync(string id)
        {
            if (id != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/item/{id}");
            }

            return null;
        }

        public async Task<string> AddItemAsync(Property item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return null;
            
            var serializedItem = JsonConvert.SerializeObject(item);
            var content = new StringContent(serializedItem, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/properties/create_property/", content);
            var stringContent = await response.Content.ReadAsStringAsync();
            return stringContent.Trim();
        }

        public async Task<bool> UpdateItemAsync(Property item)
        {
            if (item == null || item.id  == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = System.Text.Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/item/{item.id}"), byteContent);

            return response.IsSuccessStatusCode ? true : false;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/item/{id}");

            return response.IsSuccessStatusCode ? true : false;
        }
    }
}
