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
        public async Task Save()
        {
            //upload picture first
            foreach (string str in _bufferPath)
            {
                if (str == string.Empty)
                    continue;
                string remote = await MediaDataStore.UploadImage(str);   
                Item.PicPaths.Add(remote);
            }
            PropertyDataStore.AddItemAsync(Item);
            
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
				for (int i = 0; i < maxPictureCount; i++)
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
