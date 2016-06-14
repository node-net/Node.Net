namespace Node.Net.View
{
    public class Properties : System.Windows.Controls.Grid, System.IDisposable
    {
        #region Construction
        public Properties()
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
            var value = KeyValuePair.GetValue(DataContext);
            if (object.ReferenceEquals(null,value))
            {
                propertyGrid.SelectedObject = null;
                return;
            }
            var idictionary = value as System.Collections.IDictionary;
            var ienumerable = value as System.Collections.IEnumerable;
            if (object.ReferenceEquals(null,idictionary) && value.GetType() != typeof(string) &&
               !object.ReferenceEquals(null,ienumerable))
            {
                var items = new System.Collections.Generic.List<object>();
                foreach (object item in ienumerable){items.Add(item);}
                propertyGrid.SelectedObjects = items.ToArray();
            }
            else
            {
                propertyGrid.SelectedObject = value;
            } 
        }

        #region Member Data
        private System.Windows.Forms.Integration.WindowsFormsHost host = null;
        private System.Windows.Forms.PropertyGrid propertyGrid = null;
        private Title title = null;
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

            title = new Title();
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
            Update();
        }

        public event System.EventHandler ValueChanged;
        void propertyGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            if(!object.ReferenceEquals(null,ValueChanged))
            {
                ValueChanged(this, new System.EventArgs());
            }
            Update();
        }
    }
}
