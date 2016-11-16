using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Controls
{
    public class PropertyControl : System.Windows.Controls.Grid, System.IDisposable
    {
        #region Construction
        public PropertyControl()
        {
            DataContextChanged += Properties_DataContextChanged;
        }
        #endregion

        void Properties_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        private INotifyPropertyChanged inotifyPropertyChanged;
        private void OnDataContextChanged()
        {
            if (ReferenceEquals(null, propertyGrid)) return;

            title.DataContext = DataContext;
            var value = Node.Net.Collections.KeyValuePair.GetValue(DataContext);
            if (ReferenceEquals(null, value))
            {
                propertyGrid.SelectedObject = null;
                return;
            }

            if(!ReferenceEquals(null, inotifyPropertyChanged))
            {
                inotifyPropertyChanged.PropertyChanged -= DataContext_PropertyChanged;
            }
            inotifyPropertyChanged = value as INotifyPropertyChanged;
            if(!ReferenceEquals(null,inotifyPropertyChanged))
            {
                inotifyPropertyChanged.PropertyChanged += DataContext_PropertyChanged;
            }

            var idictionary = value as System.Collections.IDictionary;
            var ienumerable = value as System.Collections.IEnumerable;
            if (ReferenceEquals(null, idictionary) && value.GetType() != typeof(string) &&
               !ReferenceEquals(null, ienumerable))
            {
                var items = new System.Collections.Generic.List<object>();
                foreach (object item in ienumerable) { items.Add(item); }
                propertyGrid.SelectedObjects = items.ToArray();
            }
            else
            {
                var use_adapter = false;
                if (!ReferenceEquals(null,idictionary))
                {
                    var customTypeDescriptor = value as ICustomTypeDescriptor; ;
                    if (ReferenceEquals(null, customTypeDescriptor))
                    {
                        if (value.GetType().FullName.Contains("System.Collections.Generic.Dictionary"))
                        {
                            use_adapter = true;
                        }
                    }
                }

                if (use_adapter)
                {
                    var d = new Deprecated.Collections.Dictionary();
                    Deprecated.Collections.Copier.Copy(idictionary, d);
                    propertyGrid.SelectedObject = d;
                }
                else {
                    propertyGrid.SelectedObject = value;
                }
            }
        }

        private void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // reset propertyGrid.Selected object
            var selected = propertyGrid.SelectedObject;
            propertyGrid.SelectedObject = null;
            propertyGrid.SelectedObject = selected;
        }

        #region Member Data
        private System.Windows.Forms.Integration.WindowsFormsHost host;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private TitleControl title;
        #endregion

        #region Dispose
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
        #endregion

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = System.Windows.GridLength.Auto });
            RowDefinitions.Add(new System.Windows.Controls.RowDefinition());

            title = new TitleControl();
            Children.Add(title);

            propertyGrid = new System.Windows.Forms.PropertyGrid
            {
                ToolbarVisible = false,
                LargeButtons = true
            };
            host = new System.Windows.Forms.Integration.WindowsFormsHost { Child = propertyGrid };
            Children.Add(host);
            System.Windows.Controls.Grid.SetRow(host, 1);
            //this.Content = host;
            propertyGrid.PropertyValueChanged += propertyGrid_PropertyValueChanged;
            OnDataContextChanged();
        }

        public event System.EventHandler ValueChanged;
        void propertyGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            if (ValueChanged!=null)
            {
                ValueChanged(this, new System.EventArgs());
            }
            OnDataContextChanged();
            InvalidateVisual();
        }
    }
}
