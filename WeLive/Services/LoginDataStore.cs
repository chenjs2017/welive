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

            string url = String.Format("api/user/generate_auth_cookie/?insecure=cool&username={0}&password={1}", username, password);
            var json = await client.GetStringAsync(url);
            if (json.Contains("error"))
            {
                throw new Exception(ErrorMessage.LoginFail); 
            }
            return await Task.Run(() => JsonConvert.DeserializeObject<Cookie>(json));
			
		}
    }
}
