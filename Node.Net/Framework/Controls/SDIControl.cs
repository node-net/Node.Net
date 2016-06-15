using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Framework.Controls
{
    public class SDIControl : Grid
    {
        public SDIControl()
        {
            DataContext = new Documents();
        }
        public Documents Documents
        {
            get { return DataContext as Documents; }
            set { DataContext = value; }
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

            Background = Brushes.DarkGray;
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());

            Menu = new Menu();
            // FileMenu
            var fileMenu = new MenuItem { Header = "File" };
            Menu.Items.Add(fileMenu);
            var fileOpen = new MenuItem { Header = "Open" };
            fileMenu.Items.Add(fileOpen);
            fileOpen.Click += FileOpen_Click;

            if (!object.ReferenceEquals(null, DocumentView))
            {
                DocumentView.DataContext = null;
                Children.Add(DocumentView);
                Grid.SetRow(DocumentView, 1);
            }

            var fileRecentFiles = new MenuItem { Header = "Recent Files" };
            var recentFileItems = GetRecentFileMenuItems();
            if (recentFileItems.Length > 0)
            {
                foreach(MenuItem rfitem in recentFileItems)
                {
                    fileRecentFiles.Items.Add(rfitem);
                }
                fileMenu.Items.Add(fileRecentFiles);
            }

            // ViewMenu
            var method = DocumentView.GetType().GetMethod("GetViewMenuItem");
            if (!object.ReferenceEquals(null, method))
            {
                var viewMenuItem = method.Invoke(DocumentView, null) as MenuItem;
                if (!object.ReferenceEquals(null,viewMenuItem))
                {
                    Menu.Items.Add(viewMenuItem);
                }
            }
        }

        private static MenuItem[] GetRecentFileMenuItems()
        {
            var recentFileMenuItems = new List<MenuItem>();
            return recentFileMenuItems.ToArray();
        }
        private static void AddRecentFile(string filename)
        {
            //System.Windows.Application.Current.
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            Documents.Open();
            if (!object.ReferenceEquals(null, DocumentView))
            {
                foreach (string key in Documents.Keys)
                {
                    DocumentView.DataContext = new KeyValuePair<string,object>(key, Documents[key]);
                }
            }
        }
    }
}
