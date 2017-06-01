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
            LoadItemsCommand = new Command(async () => {Items.Clear(); await ExecuteLoadItemsCommand(); });

            MessagingCenter.Subscribe<NewItemPage, Property>(this, "AddItem", (obj, item) =>
           {
			   var _item = item as Property;
                Items.Insert(0 ,_item);
           });

            MessagingCenter.Subscribe<ItemDetailPage, Property>(this, "DeleteItem", (obj, item) =>
             {
                 var _item = item as Property;
                 Items.Remove(_item);
             });
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy || (Items.Count > 0))
                return;

            IsBusy = true;
            try
            {
                var items = await ThePropertyDataStore.GetItemsAsync();

                Items.ReplaceRange(items);
                if (App.MaxImageCount == 0)
				{
					App.MaxImageCount = await TheMediaDataStore.GetMaxUploadImageCount();
				}
                if (App.CurrentUser == null )
                {
                    App.CurrentUser = await TheLoginDataStore.GetCurrentUser();
                }
            }
        
            catch (Exception ex)
            {
                if (ex.Message == ErrorMessage.NotLogin)
                {
                    App.Reload();
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
