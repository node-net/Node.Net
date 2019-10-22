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
            this.KeyDown += VisualCreateBitmapTestControl_KeyDown;
            base.OnInitialized(e);
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            Children.Add(UIElement);
            image = new Image();
            Children.Add(image);
            Grid.SetColumn(image, 1);
            UpdateImage();
        }

        private void VisualCreateBitmapTestControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
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
