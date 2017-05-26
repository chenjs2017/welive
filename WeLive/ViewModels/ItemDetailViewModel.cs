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
                return Item.PicPaths.Count < maxPictureCount;
            }
            set 
            {
                OnPropertyChanged();
            }
        }
		private Property Item { get; set; }
        public ItemDetailViewModel(Property item = null)
        {
            if (item == null)
            {
                item = new Property();
             
            }
            Title = item.title;
            Item = item;

        }
        List<bool> _bufferHas = null;
        public List<bool> HasPics
        {
            get
            {
                if (_bufferHas != null)
                {
                    return _bufferHas;
                }
                _bufferHas = new List<bool>();
				for (int i = 0; i < maxPictureCount; i++)
				{
                    if (i < Item.PicPaths.Count)
                    {
                        _bufferHas.Add(true);
                    }
                    else
                    {
                        _bufferHas.Add(false);
                    }
				}
                return _bufferHas;                    
            }
            set 
            {
                _bufferHas = value;
                OnPropertyChanged();
            }
        }

        List<String> _bufferPath = null;
        public List<String> PicPaths
		{
			get
			{
                if (_bufferPath != null)
                {
                    return _bufferPath;
                }
                _bufferPath = new List<String>();
				for (int i = 0; i < maxPictureCount; i++)
				{
					if (i < Item.PicPaths.Count)
					{
                        _bufferPath.Add(Item.PicPaths[i]);
					}
					else
					{
                        _bufferPath.Add(String.Empty);
					}
				}
                return _bufferPath;
			}
            set 
            {
                _bufferPath = value;
                OnPropertyChanged();
            }
		}

        public void AddPicture(string path)
        {
            Item.PicPaths.Add(path);
			RefreshPrperty();

		}

        public void RemovePicture(int index)
        {
            Item.PicPaths.RemoveAt(index);
            RefreshPrperty();
        }

        void RefreshPrperty()
        {
			HasPics = null;
			PicPaths = null;
			CanAddPhoto = false;
        }
    }
}
