using System;
using System.Windows;

namespace Node.Net.Controls.StackPanels
{
    public abstract class StackPanel : System.Windows.Controls.StackPanel
    {
        public StackPanel()
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal;
            DataContextChanged += _DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            Children.Clear();
            var elements = GetFrameworkElements();
            foreach (FrameworkElement element in elements)
            {
                Children.Add(element);
            }
        }

        protected abstract FrameworkElement[] GetFrameworkElements();
    }
}
