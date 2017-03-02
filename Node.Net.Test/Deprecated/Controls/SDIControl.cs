﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls
{
    public class SDIControl : Grid
    {
        public SDIControl()
        {
            DataContext = new Collections.Documents();
        }
        public Collections.Documents Documents
        {
            get { return DataContext as Collections.Documents; }
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
            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());

            Menu = new Menu();
            // FileMenu
            MenuItem fileMenu = new MenuItem() { Header = "File" };
            Menu.Items.Add(fileMenu);
            MenuItem fileOpen = new MenuItem() { Header = "Open" };
            fileMenu.Items.Add(fileOpen);
            fileOpen.Click += FileOpen_Click;

            if (!object.ReferenceEquals(null, DocumentView))
            {
                DocumentView.DataContext = null;
                Children.Add(DocumentView);
                Grid.SetRow(DocumentView, 1);
            }

            MenuItem fileRecentFiles = new MenuItem() { Header = "Recent Files" };
            MenuItem[] recentFileItems = GetRecentFileMenuItems();
            if (recentFileItems.Length > 0)
            {
                foreach (MenuItem rfitem in recentFileItems)
                {
                    fileRecentFiles.Items.Add(rfitem);
                }
                fileMenu.Items.Add(fileRecentFiles);
            }

            // ViewMenu
            MethodInfo method = DocumentView.GetType().GetMethod("GetViewMenuItem");
            if (!object.ReferenceEquals(null, method))
            {
                MenuItem viewMenuItem = method.Invoke(DocumentView, null) as MenuItem;
                if (!object.ReferenceEquals(null, viewMenuItem))
                {
                    Menu.Items.Add(viewMenuItem);
                }
            }
        }

        private MenuItem[] GetRecentFileMenuItems()
        {
            List<MenuItem> recentFileMenuItems = new List<MenuItem>();
            return recentFileMenuItems.ToArray();
        }
        private void AddRecentFile(string filename)
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
                    DocumentView.DataContext = new KeyValuePair<string, object>(key, Documents[key]);
                }
            }
        }
    }
}