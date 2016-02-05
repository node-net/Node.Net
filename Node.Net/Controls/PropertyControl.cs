﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls
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
            Update();
        }

        private void Update()
        {
            if (object.ReferenceEquals(null, propertyGrid)) return;

            title.DataContext = DataContext;
            object value = Json.KeyValuePair.GetValue(DataContext);
            if (object.ReferenceEquals(null, value))
            {
                propertyGrid.SelectedObject = null;
                return;
            }
            System.Collections.IDictionary idictionary = value as System.Collections.IDictionary;
            System.Collections.IEnumerable ienumerable = value as System.Collections.IEnumerable;
            if (object.ReferenceEquals(null, idictionary) && value.GetType() != typeof(string) &&
               !object.ReferenceEquals(null, ienumerable))
            {
                System.Collections.Generic.List<object> items = new System.Collections.Generic.List<object>();
                foreach (object item in ienumerable) { items.Add(item); }
                propertyGrid.SelectedObjects = items.ToArray();
            }
            else
            {
                bool use_adapter = false;
                if(!object.ReferenceEquals(null,idictionary))
                {
                    ICustomTypeDescriptor customTypeDescriptor = value as ICustomTypeDescriptor;;
                    if (object.ReferenceEquals(null, customTypeDescriptor))
                    {
                        if (value.GetType().FullName.Contains("System.Collections.Generic.Dictionary"))
                        {
                            use_adapter = true;
                        }
                    }
                }
                
                if (use_adapter)
                {
                    Collections.Dictionary d = new Collections.Dictionary();
                    Json.Copier.Copy(idictionary, d);
                    propertyGrid.SelectedObject = d;
                }
                else { 
                    propertyGrid.SelectedObject = value;
                }
            }
        }

        #region Member Data
        private System.Windows.Forms.Integration.WindowsFormsHost host = null;
        private System.Windows.Forms.PropertyGrid propertyGrid = null;
        private TitleControl title = null;
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

            RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = System.Windows.GridLength.Auto });
            RowDefinitions.Add(new System.Windows.Controls.RowDefinition());

            title = new TitleControl();
            Children.Add(title);

            propertyGrid = new System.Windows.Forms.PropertyGrid();
            propertyGrid.ToolbarVisible = false;
            propertyGrid.LargeButtons = true;
            host = new System.Windows.Forms.Integration.WindowsFormsHost() { Child = propertyGrid };
            Children.Add(host);
            System.Windows.Controls.Grid.SetRow(host, 1);
            //this.Content = host;
            propertyGrid.PropertyValueChanged += propertyGrid_PropertyValueChanged;
            Update();
        }

        public event System.EventHandler ValueChanged;
        void propertyGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            if (!object.ReferenceEquals(null, ValueChanged))
            {
                ValueChanged(this, new System.EventArgs());
            }
            Update();
        }
    }
}