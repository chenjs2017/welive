using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using System.Net;
using Xamarin.Forms;
using WeLive;
using System;
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
            set { loginUser = value;OnPropertyChanged(); }
        }
		

		

		public ICommand NotNowCommand { get; }
		public ICommand SignInCommand { get; }

		async Task SignIn()
		{
			try
			{
				IsBusy = true;
				Message = "Signing In...";

				// Log the user in
				await TryLoginAsync();
			}
			finally
			{
				Message = string.Empty;
				IsBusy = false;

				if (Settings.IsLoggedIn)
                {
                    App.GoToMainPage();
                }
                else 
                {
                    Message = "Wrong Username/password!";
                }
			}
		}

        public  async Task TryLoginAsync()
		{
           
            var cookie = await UserDataStore.DoLogin(loginUser.username, loginUser.password);
            if (cookie != null)
            {
                Settings.Cookie = cookie.cookie_name + "=" + cookie.cookie;
                Settings.UserId = cookie.user.id;
                MyHttpClient.Instance.SetCookie();
            }

		}     
    }
}
