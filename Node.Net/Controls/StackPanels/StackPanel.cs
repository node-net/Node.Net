using System;
using System.Windows;

namespace Node.Net.Controls.StackPanels
{
    public abstract class StackPanel : System.Windows.Controls.StackPanel
    {
        public StackPanel()
        {
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
            foreach (FrameworkElement element in GetFrameworkElements())
            {
                Children.Add(element);
            }
        }

        protected abstract FrameworkElement[] GetFrameworkElements();
    }
}
