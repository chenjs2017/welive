using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace WeLive
{
    class CarouselView : CarouselPage
    {
      
       	public CarouselView(String[] paths, int index)
        {
            NavigationPage.SetHasNavigationBar(this, false);
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += (s, e) =>
			{
			    Navigation.PopModalAsync();
			};
            ContentPage current = null;

			for (int i = 0; i < paths.Count(); i++)
            {
                Image img = new Image();

                img.Source = paths[i];
                img.GestureRecognizers.Add(tapGestureRecognizer);
                img.Aspect = Aspect.AspectFit;
                img.HorizontalOptions = LayoutOptions.Center;
                img.VerticalOptions = LayoutOptions.Center;


                ContentPage content = new ContentPage
                {
                    BackgroundColor = Color.Black,
                    Content = new StackLayout
                    {
                        BackgroundColor = Color.Black,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Orientation = StackOrientation.Vertical,
                        Padding = 0,

                        Children = {img
                        }
                    },
                };
                if (i == index)
                {
                    current = content;
                }
                Children.Add(content);
            }
            this.CurrentPage = current;
        }
    }
}
