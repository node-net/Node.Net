using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Node.Net.Controls
{
    public class DynamicControl : Grid
    {
        public DynamicControl()
        {
            InitializeViews();
            DataContextChanged += _DataContextChanged;
        }

        private string _viewName = "";
        public string ViewName
        {
            get { return _viewName; }
            set
            {
                if (_viewName != value)
                {
                    _viewName = value;
                    Update();
                }
            }
        }

        public FrameworkElement View
        {
            get {
                if(_views.ContainsKey(ViewName)) return _views[ViewName];
                return null;
            }
        }

        public void Print()
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintDocument(GetFixedDocument(pd, View).DocumentPaginator, "?");
            }
        }

        public void PrintPreview()
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                Window w = new Window()
                {
                    WindowState = WindowState.Maximized
                };
                DocumentViewer docView = new DocumentViewer();
                docView.Document = GetFixedDocument(pd, View);
                w.Content = docView;
                w.ShowDialog();
            }
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            Children.Clear();
            foreach (string name in _views.Keys)
            {
                RemoveVisualChild(_views[name]);
                //_views[name].DataContext = DataContext;
            }
            if (!object.ReferenceEquals(null, View))
            {
                Children.Add(View);
                //Children.Add(_views[_viewName]);
                View.DataContext = DataContext;
            }
            //_views[ViewName].DataContext = DataContext;
        }

        private Dictionary<string, FrameworkElement> _views = null;
        private void InitializeViews()
        {
            _views = new Dictionary<string, FrameworkElement>();
            Update();
        }

        private FixedDocument GetFixedDocument(PrintDialog pd, FrameworkElement view)
        {
            FixedDocument doc = new FixedDocument();
            doc.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
            FixedPage page1 = new FixedPage();
            page1.Width = doc.DocumentPaginator.PageSize.Width;
            page1.Height = doc.DocumentPaginator.PageSize.Height;


            double margin = 20;
            Grid grid = new Grid()
            {
                Width = pd.PrintableAreaWidth - margin * 2,
                Height = pd.PrintableAreaHeight - margin * 2,
                Margin = new Thickness(margin)
            };
            Border border = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(5)
            };

            FrameworkElement view_clone = Activator.CreateInstance(view.GetType()) as FrameworkElement;
            view_clone.DataContext = view.DataContext;
            grid.Children.Add(border);
            border.Child = view_clone;
            page1.Children.Add(grid);

            doc.Pages.Add(new PageContent() { Child = page1 });
            return doc;
        }
    }
}
