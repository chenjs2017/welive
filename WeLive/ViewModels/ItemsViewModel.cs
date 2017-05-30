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
            Title = "列表(Browse)";
            Items = new ObservableRangeCollection<Property>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Property>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Property;
                Items.Add(_item);
                await ThePropertyDataStore.AddItemAsync(_item);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy || App.DataUptodate)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await ThePropertyDataStore.GetItemsAsync();

                Items.ReplaceRange(items);
                App.DataUptodate = true;
                if (Settings.MaxImageCount == 0)
				{
					Settings.MaxImageCount = await TheMediaDataStore.GetMaxUploadImageCount();
				}
            }
        
            catch (Exception ex)
            {

                if (ex.Message == ErrorMessage.NotLogin)
                {
                    App.ResetCookieAndSetMainPage();
                }
                else
                {
                    MessagingCenter.Send(new MessagingCenterAlert
                    {
                        Title = "",
                        Message = "无法载入信息(Unable to load items)",
                        Cancel = "确定(OK)"
                    }, "message");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
