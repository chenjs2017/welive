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
                for (int i = 0; i < Items.Count; i ++)
                {
                    if (Items[i].id == _item.id)
                    {
                        Items.RemoveAt(i);
                        break;
                    }
                }
             });
        }

        public async Task<Property> RefreshProperty(Property item)
        {
            return await ThePropertyDataStore.GetItem(item.id);
        }
        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy || (Items.Count > 0))
                return;

            IsBusy = true;
            try
            {
				await InitOptions();
				var items = await ThePropertyDataStore.GetItemsAsync(null);
				Items.ReplaceRange(items);
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
                        Message = ErrorMessage.GetMessage(ex.Message ),
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
