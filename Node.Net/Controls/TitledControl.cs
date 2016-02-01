using System;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class TitledControl : StackPanel
    {
        public TitledControl()
        {
            DataContextChanged += TitledControl_DataContextChanged;
        }

        private void TitledControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }
        protected virtual void OnDataContextChanged()
        {
            titleGrid.Children.Clear();
            titleGrid.Children.Add(new Label() { Content = Json.KeyValuePair.GetKey(DataContext) });
            contentGrid.Children.Clear();
            UIElement element = (UIElement)Json.KeyValuePair.GetValue(DataContext);
            if(!object.ReferenceEquals(null, element))
            {
                contentGrid.Children.Add(element);
            }
            //contentGrid.Children.Add((UIElement)Json.KeyValuePair.GetValue(DataContext));
        }

        private Grid titleGrid = new Grid();
        private Grid contentGrid = new Grid();
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Children.Add(titleGrid);
            Children.Add(contentGrid);
        }

        private string title = "";
        public string Title
        {
            get { return title; }
            set { title = value; OnDataContextChanged(); }
        }

        private object content = null;
        public object Content
        {
            get { return content; }
            set { content = value;  OnDataContextChanged(); }
        }
    }
}
