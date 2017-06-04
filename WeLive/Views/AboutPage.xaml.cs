using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace WeLive
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        
        public AboutPage()
        {
            InitializeComponent();

        }

        AboutViewModel viewModel
        {
            get 
            {
                return BindingContext as AboutViewModel;
            }
        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();
            count = 0;
            try 
            {
				await viewModel.InitOptions();
				if (viewModel.CurrentUser != null)
				{
					txtPhone.Text = viewModel.CurrentUser.phone;
					txtAddress.Text = viewModel.CurrentUser.address;
					lblUserName.Text = viewModel.CurrentUser.username;
					lblEmail.Text = viewModel.CurrentUser.email;
				}
            }
 			catch (System.Exception ex)
			{
				await DisplayAlert("", ErrorMessage.GetMessage(ex.Message), "确定(OK)");
			}
		}
        int count = 0;
        async void Save_Clicked(object sender, System.EventArgs e)
        {
            count++;
            if (count > 5)
            {
                this.txtBackAddress.IsVisible = true;
                this.btnSaveAddress.IsVisible = true;
                this.txtBackAddress.Text = Settings.BackendUrl;
            }
            viewModel.CurrentUser.phone = txtPhone.Text;
            viewModel.CurrentUser.address = txtAddress.Text;
            try
            {
                await viewModel.SaveCurrentUserInfo();
                await DisplayAlert("", "保存成功(Save succeed!)", "确定(OK)");
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("",ErrorMessage.GetMessage(ex.Message), "确定(OK)");
			}
        }

	    void SaveAddress_Clicked(object sender, System.EventArgs e)
        {
            Settings.BackendUrl = txtBackAddress.Text;    
            App.Reload();
        }


		
    }
}
