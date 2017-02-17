using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Readers
{
    [TestFixture]
    class ImageSourceReaderTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void ImageSourceReader_Transparency()
        {
            var width = 60;
            var imageSource = Reader.Default.Read(typeof(ImageSourceReaderTest), "StatusHelp") as ImageSource;
            var stackPanel = new StackPanel() { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
            stackPanel.Children.Add(new Button { Width = width,Height= width, Background = Brushes.Black, Content = new Image { Source = imageSource } });
            stackPanel.Children.Add(new Button { Width = width, Height = width, Background = Brushes.DarkGray, Content = new Image { Source = imageSource } });
            stackPanel.Children.Add(new Button { Width = width, Height = width, Background = Brushes.LightGray, Content = new Image { Source = imageSource } });
            stackPanel.Children.Add(new Button { Width = width, Height = width, Background = Brushes.White, Content = new Image { Source = imageSource } });
            new Window
            {
                Title = "ImageSourceReader_Transparency",
                WindowState = WindowState.Maximized,
                Content = stackPanel,
                Background = Brushes.DarkGray
            }.ShowDialog();
        }
    }
}
