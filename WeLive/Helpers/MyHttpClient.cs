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
    public class MyHttpClient
    {
		protected HttpClient client;
		protected HttpClientHandler handler;

		private MyHttpClient()
        {
			handler = new HttpClientHandler();
			handler.CookieContainer = new CookieContainer();
			client = new HttpClient(handler);
			client.BaseAddress = new Uri($"{Settings.BackendUrl}/");
			SetCookie();
        }
		private static MyHttpClient instance;


		public static MyHttpClient Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MyHttpClient();
				}
				return instance;
			}
		}

		public void SetCookie()
		{
			handler.CookieContainer.SetCookies(client.BaseAddress, Settings.Cookie);
		}

	}
}
