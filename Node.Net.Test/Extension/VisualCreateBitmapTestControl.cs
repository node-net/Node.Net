using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Tests.Extension
{
    class VisualCreateBitmapTestControl : Grid
    {
        public UIElement UIElement { get; set; }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            Children.Add(UIElement);
            image = new Image();
            Children.Add(image);
            Grid.SetColumn(image, 1);
            UpdateImage();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateImage();
        }
        private void UpdateImage()
        {
            if (ActualWidth > 0 && ActualHeight > 0)
            {
                image.Source = UIElement.CreateBitmapSource(ActualWidth / 2, ActualHeight);
            }
        }

        private Image image;
    }
}
