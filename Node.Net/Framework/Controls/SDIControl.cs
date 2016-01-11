using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Framework.Controls
{
    public class SDIControl : System.Windows.Controls.Grid
    {
        Documents Documents
        {
            get { return DataContext as Documents; }
        }
        private Menu menu = null;
        public Menu Menu
        {
            get { return menu; }
            set
            {
                Children.Remove(menu);
                menu = value;
                Children.Add(menu);
            }
        }

        private FrameworkElement documentView = null;
        public FrameworkElement DocumentView
        {
            get { return documentView; }
            set
            {
                Children.Remove(documentView);
                documentView = value;
            }
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());

            Menu = new Menu();
            MenuItem fileMenu = new MenuItem() { Header = "File" };
            Menu.Items.Add(fileMenu);
            MenuItem fileOpen = new MenuItem() { Header = "Open" };
            fileMenu.Items.Add(fileOpen);
            fileOpen.Click += FileOpen_Click;

            if (!object.ReferenceEquals(null, DocumentView))
            {
                Children.Add(DocumentView);
                Grid.SetRow(DocumentView, 1);
            }
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            Documents.Open();
            if (!object.ReferenceEquals(null, DocumentView))
            {
                foreach (string key in Documents.Keys)
                {
                    DocumentView.DataContext = Documents[key];
                }
            }
        }
    }
}
