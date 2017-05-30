using System.Collections.Generic;

using Xamarin.Forms;
using System;
using System.Net;
using System.Net.Http;

namespace WeLive
{
    public partial class App : Application
    {
        public static bool DataUptodate { get; set; }

        public App()
        {
            InitializeComponent();
            DependencyService.Register<PropertyDataStore>();
            DependencyService.Register<UserDataStore>();
            DependencyService.Register<MediaDataStore>();

            App.DataUptodate = false;
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

        public static void ResetCookieAndSetMainPage()
        {
            DataUptodate = false;   
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
                        Title = "设置(Settings)",
                        Icon = Device.OnPlatform("tab_about.png", null, null)
                    },
                }
            };
        }
    }
}
