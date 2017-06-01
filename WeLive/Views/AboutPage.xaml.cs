﻿using System;
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
            lblUserName.Text = viewModel.CurrentUser.username;
            lblEmail.Text = viewModel.CurrentUser.email;
		}

        async void Save_Clicked(object sender, System.EventArgs e)
        {
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
    }
}
