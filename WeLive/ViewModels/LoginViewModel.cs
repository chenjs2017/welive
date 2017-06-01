using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using System.Net;
using Xamarin.Forms;
using WeLive;
using System;
using System.Diagnostics;
namespace WeLive
{
    public class LoginViewModel : BaseViewModel
    {
		public LoginViewModel()
		{
			SignInCommand = new Command(async () => await SignIn());
            NotNowCommand = new Command(() => Device.OpenUri(new Uri(Settings.BackendUrl  + "/wp-login.php?action=register")));

		}

        User loginUser = new User();
        public User LoginUser 
        {
            get { return loginUser; }
        }
		
        public ICommand NotNowCommand { get; }
		public ICommand SignInCommand { get; }

        async Task SignIn()
        {
            try
            {
                IsBusy = true;
                Message = "登录中...(Signing In...)";

                // Log the user in
                var cookie = await TheLoginDataStore.DoLogin(loginUser.username, loginUser.password);
                if (cookie != null)
                {
                    Settings.Cookie = cookie.cookie_name + "=" + cookie.cookie;
                    Settings.UserId = cookie.user.id;
                    MyHttpClient.Instance.SetCookie();
                    App.GoToMainPage();
                }

            }

            catch (Exception ex)
            {
                Message = ErrorMessage.GetMessage(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
