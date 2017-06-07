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
            NotNowCommand = new Command(() => App.Current.MainPage = new NavigationPage(new Resister()));

		}

        User currUser = new User();
        public User CurrUser 
        {
            get { return currUser; }
        }
		
        public ICommand NotNowCommand { get; }
		public ICommand SignInCommand { get; }

        public async Task Register()
        {
            await TheLoginDataStore.RegisterUser(currUser);
        }
        async Task SignIn()
        {
            try
            {
                IsBusy = true;
                Message = "登录中...(Signing In...)";

                if (string.IsNullOrEmpty(currUser.username) || string.IsNullOrEmpty(currUser.password))
                {
                    Message = "请输入用户名／口令(Please input username/password)";
                }
                else
                {
                    // Log the user in
                    var cookie = await TheLoginDataStore.DoLogin(currUser.username, currUser.password);
                    if (cookie != null)
                    {
                        Settings.Cookie = cookie.cookie_name + "=" + cookie.cookie;
                        Settings.UserId = cookie.user.id;
                        MyHttpClient.Instance.SetCookie();
                        App.GoToMainPage();
                    }
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
