using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

using Xamarin.Forms;

namespace WeLive
{
    public class ItemDetailViewModel : BaseViewModel
    {
		public Command OpenWebCommand { get; }

	
		public Property Item { get; set; }
        public ItemDetailViewModel(Property item = null)
        {
            if (item == null)
            {
                item = new Property();
            }
			Title = item.title;
			Item = item;

             OpenWebCommand = new Command(() => Device.OpenUri(new Uri(item.url)));

        }
      
        public async Task<bool> Delete()
        {
            try 
            {
                bool result = await ThePropertyDataStore.DeleteItemAsync(Item.id);
                if (result)
                {
					App.DataUptodate = false;
				}
                return result;
            }
            catch
            {
                return false;
            }
           
        }

        public async Task<bool> Save()
        {
            if (IsBusy)
                return false;

			IsBusy = true;
            Message = string.Empty;
			if (String.IsNullOrEmpty(Item.title))
			{
                Message += "请输入标题（please input title)";
			}
			if (String.IsNullOrEmpty(Item.content))
			{
                Message += "\r\n请输入内容（please input content)";

			}
            if (_bufferPath.Count == 0)
			{
                Message += "\r\n请上传图片（please upload pictures)";
			}

            if (!string.IsNullOrEmpty(Message))
            {
                IsBusy = false;
                return false;
            }

            try
            {
                Message = "正在连接服务器(connecting)...";
                string id = await ThePropertyDataStore.AddItemAsync(Item);
                int i = 0;
                foreach (string str in _bufferPath)
                {
                    if (str == string.Empty)
                        break;
                    Message = String.Format("上传第{0}张图片(uploading image {0})", i + 1);
                    string attachmentID = await TheMediaDataStore.UploadImage(str, id, i.ToString());
                    i++;
                }
                App.DataUptodate = false;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                IsBusy = false;
                Message = string.Empty;
            }

		}

        List<String> _bufferPath = null;
        public List<String> PicPaths
		{
			get
			{
                if (_bufferPath != null)
                    return _bufferPath;

                _bufferPath = new List<string>();  
                if (Item != null && Item.attachments != null)
				{
					foreach (var att in Item.attachments)
					{
						if (att.images != null &&
							att.images.full != null &&
							!string.IsNullOrEmpty(att.images.full.url)
							)
						{
							_bufferPath.Add(att.images.full.url);
						}
					}
				}
                return _bufferPath;
			}
		}

        public bool CanAddPic
        {
            get 
            {
				return _bufferPath.Count < Settings.MaxImageCount;
			}
        }

        public int ImageRows
        {
            get
            {
                return (Settings.MaxImageCount - 1) / Settings.ImagesPerRow + 1;
            }
        }

        public void AddPicture(string path)
        {
            _bufferPath.Add(path);

		}

        public void RemovePicture(int index)
        {
            _bufferPath.RemoveAt(index);
        }

		public bool IsPublished
		{
			get
			{
				return  Item.status.Equals("publish");
			}
		}
		public bool NotPublished
		{
			get
			{
				return !IsPublished;
			}
		}
		public string DisplayStatus
		{
			get
			{
				return IsPublished ? "发布成功(Published)" : "正在审核(Pending)";
			}
		}

    }
}
