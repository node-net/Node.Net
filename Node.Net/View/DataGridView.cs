namespace Node.Net.View
{
    public class DataGridView : System.Windows.Controls.UserControl
    {
        #region Construction
        public DataGridView()
        {
            DataContextChanged += Properties_DataContextChanged;
        }

        void Properties_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (object.ReferenceEquals(null, dataGridView)) return;

            var value = KeyValuePair.GetValue(DataContext);
            var idictionary = value as System.Collections.IDictionary;
            var ienumerable = value as System.Collections.IEnumerable;
            if (object.ReferenceEquals(null,idictionary) && value.GetType() != typeof(string) &&
               !object.ReferenceEquals(null,ienumerable))
            {
                var items = new System.Collections.Generic.List<object>();
                foreach (object item in ienumerable){items.Add(item);}
                dataGridView.DataSource = items;
            }
            else
            {
                dataGridView.DataSource = value;
            }
        }
        #endregion

        #region Member Data
        private System.Windows.Forms.Integration.WindowsFormsHost host = null;
        private System.Windows.Forms.DataGridView dataGridView = null;
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
                dataGridView.Dispose();
                host.Dispose();
            }
        }
        #endregion

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);
            dataGridView = new System.Windows.Forms.DataGridView();
            host = new System.Windows.Forms.Integration.WindowsFormsHost { Child = dataGridView };
            this.Content = host;
            dataGridView.CellValueChanged += dataGridView_CellValueChanged;
            Update();
        }

        void dataGridView_CellValueChanged(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (!object.ReferenceEquals(null, ValueChanged))
            {
                ValueChanged(this, new System.EventArgs());
            }
        }

        public event System.EventHandler ValueChanged;
    }
}
