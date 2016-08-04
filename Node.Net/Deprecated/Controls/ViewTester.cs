using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Deprecated.Controls
{
    public class ViewTester : System.Windows.Controls.Grid
    {
        public static void ShowDialog(object models, FrameworkElement view)
        {
            var window = new System.Windows.Window
            {
                Title = GetTitle(view),
                Content = new ViewTester(models, view),
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        public static void ShowDialog(object models, FrameworkElement[] views, string title = nameof(ViewTester))
        {
            var window = new System.Windows.Window
            {
                Title = title,
                Content = GetGrid(models, views),
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        public static Grid GetGrid(object models, FrameworkElement[] views)
        {
            var grid = new Grid();

            var modelsDictionary = models as IDictionary;
            if (object.ReferenceEquals(null, modelsDictionary)) return grid;

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(new Label { Content = nameof(Model), Background = Brushes.LightGray });
            var viewLabel = new Label { Content = nameof(View), Background = Brushes.LightGray };
            grid.Children.Add(viewLabel);
            Grid.SetColumn(viewLabel, 1);

            foreach (string key in modelsDictionary.Keys)
            {
                foreach (FrameworkElement view in views)
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    var modelLabel = new Label { Content = key };
                    grid.Children.Add(modelLabel);
                    // Grid.SetRow(modelLabel,)
                }
            }
            return grid;
        }
        private static string GetTitle(FrameworkElement frameworkelement)
        {
            var titleInfo = frameworkelement.GetType().GetProperty(nameof(Title));
            if (!object.ReferenceEquals(null, titleInfo))
            {
                return titleInfo.GetValue(frameworkelement).ToString();
            }
            return frameworkelement.GetType().FullName;
        }

        private object _models;
        private FrameworkElement _view;
        private object _selectedModel;
        public ViewTester(object models, FrameworkElement view)
        {
            _models = models;
            _view = view;
        }

        public string Title
        {
            get
            {
                if (!object.ReferenceEquals(null, _view))
                {
                    return _view.GetType().FullName;
                }
                return GetType().FullName;
            }
        }

        private Selector _selector;
        private System.Windows.Controls.Grid _viewGrid;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());

            _selector = new Selector("Models", _models);
            _selector.SelectionChanged += _selector_SelectionChanged;
            Children.Add(_selector);

            _viewGrid = new System.Windows.Controls.Grid();
            Children.Add(_viewGrid);
            Grid.SetRow(_viewGrid, 1);

            Update();
        }

        private void _selector_SelectionChanged(object selected_item)
        {
            _selectedModel = selected_item;
            Update();
        }

        private void Update()
        {
            if (!object.ReferenceEquals(null, _viewGrid))
            {
                if (!_viewGrid.Children.Contains(_view)) _viewGrid.Children.Clear();
                if (!object.ReferenceEquals(null, _view))
                {
                    if (!_viewGrid.Children.Contains(_view)) _viewGrid.Children.Add(_view);
                    _view.DataContext = _selectedModel;
                }
            }
        }
    }
}
