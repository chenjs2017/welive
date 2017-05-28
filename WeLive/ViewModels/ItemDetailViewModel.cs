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
        int maxPictureCount = 9;


        public bool CanAddPhoto 
        { 
            get
            {
                return _bufferPath[_bufferPath.Count -1 ] == String.Empty;
            }
            set 
            {
                OnPropertyChanged();
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
			if (!HasPics[0])
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
                string id = await PropertyDataStore.AddItemAsync(Item);
                int i = 0;
                foreach (string str in _bufferPath)
                {
                    if (str == string.Empty)
                        break;
                    string attachmentID = await MediaDataStore.UploadImage(str, id, i.ToString());
                    i++;
                }
                return true;
            }
            catch(System.Exception ex)
            {
                return false;
            }
            finally
            {
                IsBusy = false;
                Message = string.Empty;
            }

		}
        public Property Item { get; set; }
        public ItemDetailViewModel(Property item = null)
        {
            if (item == null)
            {
                item = new Property();
             
            }
            Title = item.title;
            Item = item;

        }

        List<bool> _hasPics;
        public List <bool> HasPics
        {
            get 
            {
                if (_hasPics != null)
                {
                    return _hasPics;
                }
                _hasPics = new List<bool>();
                foreach(string path in PicPaths)
                {
                    _hasPics.Add(path != string.Empty);
                }
                return _hasPics;
            }
            set
            {
                _hasPics = null;
                OnPropertyChanged();
            }
        }

        List<String> _bufferPath;
        public List<String> PicPaths
		{
			get
			{
                if (_bufferPath != null)
                {
                    return _bufferPath;
                }

                _bufferPath = new List<String>();
				
                if (Item.attachments != null )
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

                int count = _bufferPath.Count;
                for (int i = count; i < maxPictureCount; i++)
				{
					_bufferPath.Add(String.Empty);
				}
                return _bufferPath;
			}
            set 
            {
                OnPropertyChanged();
            }
		}

        public void AddPicture(string path)
        {
            for (int i = 0; i < _bufferPath.Count; i++)
            {
                if (_bufferPath[i] == string.Empty)
                {
                    _bufferPath[i] = path;
                    break;
                }                
            }
			RefreshPrperty();

		}

        public void RemovePicture(int index)
        {
            for (int i = index; i < _bufferPath.Count - 1; i++)
            {
                _bufferPath[i] = _bufferPath[i + 1];
            }
            _bufferPath[_bufferPath.Count - 1] = string.Empty;
            RefreshPrperty();
        }

        void RefreshPrperty()
        {
			PicPaths = null;
			CanAddPhoto = false;
            HasPics = null;
        }
    }
}
