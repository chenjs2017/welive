using System.Collections.Generic;

using Xamarin.Forms;
using System;
using System.Net;
using System.Net.Http;

namespace WeLive
{
    public partial class App : Application
    {
        public static string BackendUrl = "http://13.58.22.130/wp1";

        public static IDictionary<string, string> LoginParameters => null;

        public App()
        {
            InitializeComponent();
            DependencyService.Register<PropertyDataStore>();
            DependencyService.Register<UserDataStore>();
            DependencyService.Register<MediaDataStore>();
            SetMainPage();
        }

        public static void SetMainPage()
        {
			
            if (Settings.IsLoggedIn)
            {
                GoToMainPage();
            }
            else 
            {
                Current.MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = (Color)Current.Resources["Primary"],
                    BarTextColor = Color.White
                };
            }
           
        }

        public static void ResetCookie()
        {
            Settings.ResetCookie();
            SetMainPage();
        }

        public static void GoToMainPage()
        {
            Current.MainPage = new TabbedPage
            {
                Children = {
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "浏览(Browse)",
                        Icon = Device.OnPlatform("tab_feed.png", null, null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "关于(About)",
                        Icon = Device.OnPlatform("tab_about.png", null, null)
                    },
                }
            };
        }
    }
}
