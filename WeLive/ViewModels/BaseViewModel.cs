using Xamarin.Forms;
using System.Threading;
using System;
using System.Threading.Tasks;
namespace WeLive
{
    public class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        public PropertyDataStore ThePropertyDataStore = new PropertyDataStore();
        public LoginDataStore TheLoginDataStore = new LoginDataStore();
        public MediaDataStore TheMediaDataStore = new MediaDataStore();
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

		string message = string.Empty;
		public string Message
		{
			get { return message; }
			set { message = value; OnPropertyChanged(); }
		}
        public async Task InitOptions()
        {
			if (App.MaxImageCount == 0)
			{
				App.MaxImageCount = await TheMediaDataStore.GetMaxUploadImageCount();
			}
			if (App.CurrentUser == null)
			{
				App.CurrentUser = await TheLoginDataStore.GetCurrentUser();
			}
        }
    }
}
