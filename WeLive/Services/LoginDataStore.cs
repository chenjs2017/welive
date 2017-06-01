using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;

namespace WeLive
{
    public class LoginDataStore :BaseDataStore
    {
        
        public async Task<Cookie> DoLogin(string username, string password)
		{
            if (!CrossConnectivity.Current.IsConnected)
            {
                throw new Exception(ErrorMessage.NetworkIssue); 
            }

            string url = String.Format("api/properties/generate_auth_cookie/?username={0}&password={1}", username, password);
            var json = await client.GetStringAsync(url);
            if (ErrorMessage.ErrorContainCode(json, ErrorMessage.ServerReturnError))
            {
                throw new Exception(ErrorMessage.LoginFail); 
            }
            return await Task.Run(() => JsonConvert.DeserializeObject<Cookie>(json));
		}

		public async Task<User> GetCurrentUser()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				throw new Exception(ErrorMessage.NetworkIssue);
			}

            string url = String.Format("api/properties/get_userinfo/");
			var json = await client.GetStringAsync(url);
            ErrorMessage.CheckRespond(json);
			return await Task.Run(() => JsonConvert.DeserializeObject<User>(json));
		}

        public async Task SaveCurrentUser(User user)
        {
			if (!CrossConnectivity.Current.IsConnected)
			{
				throw new Exception(ErrorMessage.NetworkIssue);
			}

			var serializedItem = JsonConvert.SerializeObject(user);
			var content = new StringContent(serializedItem, Encoding.UTF8, "application/json");

			var response = await client.PostAsync($"api/properties/save_userinfo/", content);
			var stringContent = await response.Content.ReadAsStringAsync(); 
            ErrorMessage.CheckRespond(stringContent);
        }
    }
}
