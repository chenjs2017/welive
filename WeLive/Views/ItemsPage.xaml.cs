using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace WeLive
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Property;
            if (item == null)
                return;
            try 
            {
				Property newProperty = await viewModel.RefreshProperty(item);
				await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(newProperty)));
			}
            catch (System.Exception ex)
            {
                await DisplayAlert("", ErrorMessage.GetMessage(ex.Message),"确定(OK)");
            }
           
            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;
            await Navigation.PushAsync(new NewItemPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.ExecuteLoadItemsCommand();
      }

    }
}
