using Xamarin.Forms;

namespace WeLive
{
    public class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        public PropertyDataStore PropertyDataStore => DependencyService.Get<PropertyDataStore>();
        public UserDataStore UserDataStore => DependencyService.Get<UserDataStore>();
        public MediaDataStore MediaDataStore => DependencyService.Get<MediaDataStore>();
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
    }
}
