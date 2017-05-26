using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace WeLive
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Property> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableRangeCollection<Property>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Property>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Property;
                Items.Add(_item);
                await PropertyDataStore.AddItemAsync(_item);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await PropertyDataStore.GetItemsAsync(true);
                Items.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    App.ResetCookie();
                }
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
