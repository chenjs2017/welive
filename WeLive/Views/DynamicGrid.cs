﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Diagnostics;
namespace WeLive
{
    public class DynamicGrid
    {
        Grid _grid;
        int _rowCount;
        int _colCount;
        List<Image> _images = new List<Image>();
        List<StackLayout> _stacks = new List<StackLayout>();
        string[] _imagePath;

		bool hasButton = false;
		int imageGridHeigh
		{
			get
			{
                if (hasButton)
                {
                    return 145;
                } else 
                {
                    return 92;
                }
			}
		}

        public DynamicGrid(Grid grid, int rowCount, int colCount)
        {
            _grid = grid;
            _rowCount = rowCount;
            _colCount = colCount;
        }

        public List<Button> InitImageGrid(bool showRemoveButton, EventHandler onRemove, string[] imgPath, INavigation navigation)
        {
            List<Button> list = new List<Button>();
            hasButton = showRemoveButton;
            _imagePath = imgPath;
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += (s, e) =>
			{
				int index = _images.IndexOf(s as Image);
				CarouselView car = new CarouselView(_imagePath, index);
				navigation.PushModalAsync(car);
			};

            for (int i = 0; i < _rowCount; i++)
            {
                _grid.RowDefinitions.Add(new RowDefinition());

                for (int j = 0; j < _colCount; j++)
                {
                    if (i == 0)
                    {
                        _grid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                    var img = new Image();

                    img.Aspect = Aspect.AspectFill;
                    img.GestureRecognizers.Add(tapGestureRecognizer);

					StackLayout stack = new StackLayout();

                    _stacks.Add(stack);
					stack.Children.Add(img);
                    _images.Add(img);
                    _grid.Children.Add(stack,j,i);
					int imgIndex = i * _colCount + j;
					if (imgPath != null && imgIndex < imgPath.Length)
					{
						img.WidthRequest = 90;
						img.HeightRequest = 90;
                        img.Source = imgPath[imgIndex];
                        stack.IsVisible = true;
					}
                    else 
                    {
						img.WidthRequest = 0;
						img.HeightRequest = 0;
						stack.IsVisible = false;
                    }

					if (showRemoveButton)
					{
						var btn = new Button();
						btn.Text = "删除 del";
                        btn.FontSize = Device.GetNamedSize(NamedSize.Micro,typeof(Button));
						btn.Clicked += onRemove;
						stack.Children.Add(btn);
						list.Add(btn);
					}
                }
            }
            RemoveEmptyGrid();
            return list;
        }

       

        public void RefreshGrid(string[]imgPath)
        {
            _imagePath = imgPath;
            for (int i = 0; i < _stacks.Count; i ++)
            {
                if (i < imgPath.Length && !string.IsNullOrEmpty(imgPath[i]))
                {
                    _stacks[i].IsVisible = true;
                    _images[i].Source  = imgPath[i];
					_images[i].WidthRequest = 90;
					_images[i].HeightRequest = 90;
                }
                else 
                {
                    _images[i].Source = string.Empty;
                    _stacks[i].IsVisible = false;
					_images[i].WidthRequest = 0;
					_images[i].HeightRequest = 0;
                    break;
                }
            }
            RemoveEmptyGrid();
        }

        void RemoveEmptyGrid()
        {
            for (int i = 0; i < this._rowCount; i ++)
            {
				int imgIndex = i * _colCount ;
                if (imgIndex >= _imagePath.Length || String.IsNullOrEmpty(this._imagePath[imgIndex]))
                {
                    _grid.RowDefinitions[i].Height = 0;
                }
                else 
                {
                    _grid.RowDefinitions[i].Height = imageGridHeigh;
				}
            }
        }
    }
}
