using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeLive
{
	public class CustomFields
	{
		public string[] webbupointfinder_items_address { get; set; }
	}

	public class Attachment
	{
		public PropertyImage images { get; set; }
	}

	public class ThumbnailImage
	{
		public ImageInfo thumbnail { get; set; }
	}
	public class PropertyImage
	{
		public ImageInfo full { get; set; }
	}
	public class ImageInfo
	{
		public string url { get; set; }
	}

	public class Property : ObservableObject
	{
		public string id { get; set; }
		public string title { get; set; }
		public string status { get; set; }
		public string url { get; set; }
        string _address;
        public string address 
        { 
            get
            {
                if (string.IsNullOrEmpty(_address) &&
                    custom_fields != null &&
                    custom_fields.webbupointfinder_items_address != null &&
                    custom_fields.webbupointfinder_items_address.Length > 0)
                {
                    _address = custom_fields.webbupointfinder_items_address[0];
                }
                return _address;
            }
            set
            {
                _address = value;    
            }
        }

		string _content ;
		public string content
		{
			get
			{
				return _content.Replace("<p>", "").Replace("</p>", "");
			}
			set
			{
				_content = value;
			}
		}
		public CustomFields custom_fields { get; set; }
		public List<Attachment> attachments { get; set; }
		public ThumbnailImage thumbnail_images { get; set; }

	}

}
