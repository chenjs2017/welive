using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Diagnostics;
using System.IO;
namespace WeLive
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class NewItemPage : ContentPage
    {
       
        public List<Button> listButton;
        ItemDetailViewModel viewModel;
        DynamicGrid dynamicGrid;
		public NewItemPage()
		{
			InitializeComponent();
            BindingContext = viewModel = new ItemDetailViewModel();
		}


        void InitImageControls()
        {
            dynamicGrid = new DynamicGrid(this.picGrid, viewModel.ImageRows, Settings.ImagesPerRow);
            listButton = dynamicGrid.InitImageGrid(true ,remove_click, viewModel.PicPaths.ToArray(),Navigation);
        }

        IMedia current = null;
		protected override void OnAppearing()
		{
			base.OnAppearing();
            if (dynamicGrid==null)
            {
                InitImageControls();
            }
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, Settings.ImageDirectory);
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                dir.Create(); 
            }
        }
		async void btnBrowsePhotos_Click()
        {
            if (current == null)
            {
                await CrossMedia.Current.Initialize();
                current = CrossMedia.Current;
            }

            if (!current.IsPickPhotoSupported)
			{
                await DisplayAlert("", "无法选取图片(Can't pic photos)", "确认(OK)");
				return;
			}
            MediaFile file = await current.PickPhotoAsync(new PickMediaOptions
			{
                

                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = Settings.PhotosSize,				
                CompressionQuality = Settings.Qulity

			});
            //copy file from temp to sample
            if (file != null)
            {
                string newPath = Path.GetDirectoryName(file.Path);
                newPath = newPath.Replace("/temp", "/" + Settings.ImageDirectory +"/");
                newPath += Guid.NewGuid().ToString();
                newPath += Path.GetExtension(file.Path);
				System.IO.File.Copy(file.Path, newPath, true);
				viewModel.AddPicture(newPath);
                refreshControl();
            }
            		
		}

       
         async void btnTakePhotos_Click()
        {
			if (current == null)
			{
				await CrossMedia.Current.Initialize();
                current = CrossMedia.Current;
			}

            if (!current.IsCameraAvailable || !current.IsTakePhotoSupported)
			{
                await DisplayAlert("", "没有拍照设备(No camera available.)", "确认(OK)");
				return;
			}
            MediaFile file = await current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = Settings.ImageDirectory,
                Name =Guid.NewGuid().ToString() + ".jpg",
                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = Settings.PhotosSize,
                CompressionQuality = Settings.Qulity
                  
			});
            if (file != null)
            {
				viewModel.AddPicture(file.Path);
                refreshControl();
			}
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            bool flag = await viewModel.Save();
            if (flag)
            {
                MessagingCenter.Send(this, "AddItem", viewModel.Item);
				await DisplayAlert("","保存成功(succeed!)", "确认(OK)");
				await Navigation.PopToRootAsync();
			}
        }

        void remove_click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listButton.Count; i ++)
            {
                if (listButton[i] == sender)
                {
                    viewModel.RemovePicture(i);
                    break;
                }
            }
            refreshControl();
        }

        void refreshControl()
        {
            this.dynamicGrid.RefreshGrid(viewModel.PicPaths.ToArray());
            this.btnTakePhoto.IsEnabled = viewModel.CanAddPic;
            this.btnBrowsePhoto.IsEnabled = viewModel.CanAddPic;
		}
   }
}
