using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeLive
{
    public class Property : ObservableObject
    {
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string address { get; set; }

        List<String> _picPaths = new List<string>();
        public List<String> PicPaths
        {
            get
            {
                return _picPaths;
            }
        }
	}
}
