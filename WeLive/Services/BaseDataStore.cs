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
		
		protected HttpClient client
        {
            get 
            {
                return MyHttpClient.Instance.Client;
            }
        }

    }
	
}
