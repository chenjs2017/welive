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
		public URL thumbnail { get; set; }
	}
	public class PropertyImage
	{
		public URL full { get; set; }
	}
	public class URL
	{
		public string url { get; set; }
	}

	public class Property : ObservableObject
	{
		public string id { get; set; }
		public string title { get; set; }
		public string status { get; set; }
		public string url { get; set; }

		string _content = "";
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
