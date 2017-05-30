using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace WeLive
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "设置(Settings)";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri(Settings.BackendUrl)));
            SignOutCommand = new Command(() => { App.ResetCookieAndSetMainPage();});
        }

  
        public ICommand OpenWebCommand { get; }
        public ICommand SignOutCommand { get; }
    }
}
