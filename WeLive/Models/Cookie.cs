using System;
namespace WeLive
{
    public class Cookie : ObservableObject
    {
		public string cookie {get;set;}
		public string cookie_name { get; set; }
        public User user { get; set; }
    }
}
