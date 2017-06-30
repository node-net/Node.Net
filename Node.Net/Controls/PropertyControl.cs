using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class PropertyControl : UserControl, IDisposable
    {
        public PropertyControl()
        {
            Content = new System.Windows.Forms.Integration.WindowsFormsHost
            {
                Child = propertyGrid
            };
            DataContextChanged += PropertyControl_DataContextChanged;
        }

        private void PropertyControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                var npc = DataContext as INotifyPropertyChanged;
                if (npc != null)
                {
                    npc.PropertyChanged += Npc_PropertyChanged;
                }
                if (DataContext.HasPropertyValue("SelectedObject"))
                {
                    SelectedObject = DataContext.GetPropertyValue<object>("SelectedObject");
                    return;
                }
            }
            SelectedObject = DataContext;
        }
        private void Npc_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                if (DataContext.HasPropertyValue("SelectedObject"))
                {
                    SelectedObject = DataContext.GetPropertyValue<object>("SelectedObject");
                }
            }
        }

        public object SelectedObject
        {
            get { return GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register(nameof(SelectedObject), typeof(object), typeof(PropertyControl), new FrameworkPropertyMetadata(OnProjectChanged));

        private static void OnProjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var propertyGrid = (d as PropertyControl).PropertyGrid as System.Windows.Forms.PropertyGrid;
            propertyGrid.SelectedObject = e.NewValue;
        }

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
                (Content as System.Windows.Forms.Integration.WindowsFormsHost).Dispose();
            }
        }
        public object PropertyGrid { get { return propertyGrid; } }
        private System.Windows.Forms.PropertyGrid propertyGrid = new System.Windows.Forms.PropertyGrid
        {
            ToolbarVisible = false,
            LargeButtons = true
        };
    }
}
