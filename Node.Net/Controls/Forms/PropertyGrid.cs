using System;
using System.Collections;
using System.Windows;
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

        private string GetLabelText()
        {
            var type = "";
            var key = "";
            var value = Internal.KeyValuePair.GetValue(DataContext);
            if(value != null)
            {
                type = value.GetType().ToString();
                var dictionary = value as IDictionary;
                if (dictionary != null && dictionary.Contains("Type")) type = dictionary["Type"].ToString();
                if(Internal.KeyValuePair.IsKeyValuePair(DataContext))
                {
                    key = Internal.KeyValuePair.GetKey(DataContext).ToString();
                }
            }
            return $"{type} {key}".Trim();
        }
        private void Update()
        {
            titleLabel.Content = GetLabelText();

            if (propertyGrid != null)
            {
                propertyGrid.SelectedObject = Node.Net.Controls.Internal.KeyValuePair.GetValue(DataContext);
            }
        }

        private Label titleLabel = new Label();
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

            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());

            Children.Add(titleLabel);

            propertyGrid = new System.Windows.Forms.PropertyGrid
            {
                ToolbarVisible = false,
                LargeButtons = true
            };
            propertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
            host = new System.Windows.Forms.Integration.WindowsFormsHost { Child = propertyGrid };
            Children.Add(host);
            Grid.SetRow(host, 1);
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
