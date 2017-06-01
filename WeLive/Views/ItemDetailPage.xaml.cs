using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace WeLive
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            DynamicGrid dynamicGrid = new DynamicGrid(this.picGrid, viewModel.ImageRows, Settings.ImagesPerRow);
            dynamicGrid.InitImageGrid(false, null, viewModel.PicPaths.ToArray(),Navigation);
        }

        async void Delete_Clicked(object sender, System.EventArgs e)
        {
            try 
            {
                await viewModel.Delete();
				MessagingCenter.Send(this, "DeleteItem", viewModel.Item);
				await DisplayAlert("","删除成功(succeed!)","确认(OK)");
				await Navigation.PopToRootAsync();
			}
            catch (System.Exception ex)
            {
                await DisplayAlert("", ErrorMessage.GetMessage(ex.Message ), "确认(OK)");            
            }

		}
    }
}
