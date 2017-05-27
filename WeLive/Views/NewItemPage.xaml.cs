using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace WeLive
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class NewItemPage : ContentPage
    {
       
        public List<Image> listImage = new List<Image>();
        public List<Button> listButton = new List<Button>();
        ItemDetailViewModel viewModel;
       
		public NewItemPage()
		{
            //string a = AppResources.AddButton;
			InitializeComponent();
            BindingContext = viewModel = new ItemDetailViewModel();
            InitImageControls();
		}

        void InitImageControls()
        {
			this.listImage.Add(image00);
			this.listImage.Add(image01);
			this.listImage.Add(image02);
			this.listImage.Add(image10);
			this.listImage.Add(image11);
			this.listImage.Add(image12);
			this.listImage.Add(image20);
			this.listImage.Add(image21);
			this.listImage.Add(image22);

			this.listButton.Add(btn00);
			this.listButton.Add(btn01);
			this.listButton.Add(btn02);
			this.listButton.Add(btn10);
			this.listButton.Add(btn11);
			this.listButton.Add(btn12);
			this.listButton.Add(btn20);
			this.listButton.Add(btn21);
			this.listButton.Add(btn22);
        }

         IMedia current = null;

		async void btnBrowsePhotos_Click()
        {
            if (current == null)
            {
                await CrossMedia.Current.Initialize();
                current = CrossMedia.Current;
            }


            if (!current.IsPickPhotoSupported)
			{
                await DisplayAlert("无法选取图片(Can't pic photos)", "无法选取图片(Can't pic photos)", "确认(OK)");
				return;
			}
            MediaFile file = await current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
			{
				CustomPhotoSize = 30,				
				CompressionQuality = 82

			});
            //copy file from temp to sample
            if (file != null)
            {
                
                string newPath = file.Path.Replace("/temp/","/");
				System.IO.File.Copy(file.Path, newPath, true);
				viewModel.AddPicture(newPath);
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
                await DisplayAlert("没有拍照设备(No Camera)", "没有拍照设备(No camera available.)", "确认(OK)");
				return;
			}
            MediaFile file = await current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				Directory = "Sample",
				Name = "test.jpg",
			    CustomPhotoSize = 30,
                AllowCropping=true,
                CompressionQuality = 82
                  
			});
            if (file != null)
            {
				viewModel.AddPicture(file.Path);

			}
        }



        async void Save_Clicked(object sender, EventArgs e)
        {

            //MessagingCenter.Send(this, "AddItem", Item);
            await viewModel.Save();
            await Navigation.PopToRootAsync();
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
        }

        async void getlocation_click(object sender, EventArgs e)
        {
            
        }


   }
}
