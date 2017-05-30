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
    public class BaseDataStore
    {
		protected HttpClient client;
		protected HttpClientHandler handler;
        public BaseDataStore()
        {
			handler = new HttpClientHandler();
			handler.CookieContainer = new CookieContainer();
			client = new HttpClient(handler);
            //client.Timeout = new TimeSpan(5000;)
			client.BaseAddress = new Uri($"{Settings.BackendUrl}/");
			SetCookie(Settings.Cookie);
        }

		public void SetCookie(String cookie)
		{
            try 
            {
				handler.CookieContainer.SetCookies(client.BaseAddress, cookie);

			}
            catch(System.Exception ex) 
            {
                Debug.Write(ex.Message); 
            }
		}

    }
	
}
