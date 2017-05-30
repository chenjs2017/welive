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
			List<Property> items = new List<Property>();

			if ( CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/properties/get_my_properties/");
                if (json.Contains("no user"))
                {
                    throw new NotLoginException();
                }
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
                    }
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

            try
            {
                var serializedItem = JsonConvert.SerializeObject(item);
                var content = new StringContent(serializedItem, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"api/properties/create_property/", content);
                var stringContent = await response.Content.ReadAsStringAsync();
                return stringContent.Trim();
            }
            catch (System.Exception ex)
            {
                Debug.Write(ex.Message); 
                return null;
            }
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
            string url = $"api/properties/del/?post_id=" + id;
			try 
            {
				var json = await client.GetStringAsync(url);
				return true;
			}
            catch (System.Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }
    }
}
