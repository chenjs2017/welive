using System;
using Newtonsoft.Json;

namespace WeLive
{
    public class User : ObservableObject
    {
        public string id { get; set; }
    	public string username { get; set; }
        public string password { get; set; }		
    }
}
