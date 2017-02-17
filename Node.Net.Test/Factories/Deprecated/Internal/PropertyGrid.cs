using System;
using System.Windows.Controls;

namespace Node.Net.Controls.Forms
{
    public class PropertyGrid : Grid, IDisposable
    {
        public PropertyGrid()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (propertyGrid != null)
            {
                propertyGrid.SelectedObject = DataContext;
            }
        }

        private System.Windows.Forms.Integration.WindowsFormsHost host;
        private System.Windows.Forms.PropertyGrid propertyGrid;

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                propertyGrid.Dispose();
                host.Dispose();
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            propertyGrid = new System.Windows.Forms.PropertyGrid
            {
                ToolbarVisible = false,
                LargeButtons = true
            };
            propertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
            host = new System.Windows.Forms.Integration.WindowsFormsHost { Child = propertyGrid };
            Children.Add(host);
            Update();
        }
        public event System.EventHandler ValueChanged;

        private void PropertyGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new System.EventArgs());
            }
            Update();
            InvalidateVisual();
        }
    }
}
