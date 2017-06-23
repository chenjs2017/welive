using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
namespace WeLive
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Resister : ContentPage
    {
        LoginViewModel viewModel;
        public Resister()
        {
            InitializeComponent();
            BindingContext = viewModel = new LoginViewModel();
        }

        void Login_Clicked(object sender, System.EventArgs e)
        {
            App.SetMainPage();
        }
		async void Signup_Clicked(object sender, System.EventArgs e)
		{
            try 
            {
                if (IsBusy)
                    return;
                viewModel.Message = "正在注册(Registering)";
                IsBusy = true;
				ErrorMessage.ValidateEmpty(txtUserName.Text, "用户名", "username");
				ErrorMessage.ValidateEmpty(txtEmail.Text,"电子邮件","Email");
				ErrorMessage.ValidateEmail(txtEmail.Text);
				ErrorMessage.ValidateEmpty(txtPhone.Text,"电话","telephone number");
                ErrorMessage.ValidateEmpty(txtAddress.Text, "地址","address");
                ErrorMessage.ValidateEmpty(txtPassword.Text,"密码","password"); 
                ErrorMessage.ValidateEqual(txtPassword.Text,txtRePassword.Text,
                                          "两次密码输入不一致(please input same password");
				
                await viewModel.Register();
                await DisplayAlert("", "注册成功，现在用新用户登录系统(register succeed, please login the system)", "确认(OK)");
                App.SetMainPage();
			}
            catch (System.Exception ex)
            {
                await DisplayAlert("", ex.Message, "确认(OK)");
                viewModel.Message = "";
                IsBusy = false;
            }

			
            if (txtPassword.Text == "changebackaddress")
			{
				this.txtBackAddress.IsVisible = true;
				this.btnSaveAddress.IsVisible = true;
				this.txtBackAddress.Text = Settings.BackendUrl;
			}
		}

		void SaveAddress_Clicked(object sender, System.EventArgs e)
		{
			Settings.BackendUrl = txtBackAddress.Text;
            App.Reload();
		}
    }
}
