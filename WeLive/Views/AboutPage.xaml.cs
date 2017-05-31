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



		protected override void OnAppearing()
		{
			base.OnAppearing();
           
            txtPhone.Text = viewModel.CurrentUser.phone;
            txtAddress.Text = viewModel.CurrentUser.address;
            lblUserName.Text = viewModel.CurrentUser.nickname;
		}

        async void Save_Clicked(object sender, System.EventArgs e)
        {
            viewModel.CurrentUser.phone = txtPhone.Text;
            viewModel.CurrentUser.address = txtAddress.Text;
            bool result = await viewModel.SaveCurrentUserInfo();
            if (result)
            {
                await DisplayAlert("","保存成功(Save succeed!)","确定(OK)");
            }
        }
    }
}
