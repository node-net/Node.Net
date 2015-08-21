namespace Node.Net.View
{
    public class SDIApplication : System.Windows.Controls.UserControl
    {
        public SDIApplication() { }
        public SDIApplication(object value)
        {
            DataContext = value;
            DataContextChanged += On_DataContextChanged;
            update();
        }

        public object Document
        {
            get{return KeyValuePair.GetValue(DataContext);}
            set { DataContext = value; }
        }
        private System.Windows.Controls.Grid grid = null;

        private string openFileDialogFilter = "Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        public string OpenFileDialogFilter
        {
            get { return openFileDialogFilter; }
            set { openFileDialogFilter = value; }
        }

        private string fileName = "";
        private System.Type documentType = null;
        public System.Type DocumentType
        {
            get { return documentType; }
            set { documentType = value; }
        }
        private System.Windows.FrameworkElement documentView = null;
        private System.Type documentViewType = null;
        public System.Type DocumentViewType
        {
            get
            {
                return documentViewType;
            }
            set
            {
                if(!typeof(System.Windows.FrameworkElement).IsAssignableFrom(value))
                {
                    throw new System.InvalidOperationException("DocumentViewType must be assignable to System.Windows.FrameworkElement.");
                }
                documentViewType = value;
            }
        }
        private System.Windows.Controls.Menu menu = null;
        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            grid = new System.Windows.Controls.Grid();
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = System.Windows.GridLength.Auto });
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());

            menu = new System.Windows.Controls.Menu();
            System.Windows.Controls.MenuItem fileMenuItem = new System.Windows.Controls.MenuItem(){Header="File"};
            System.Windows.Controls.MenuItem fileNewMenuItem = new System.Windows.Controls.MenuItem() { Header = "New"};
            System.Windows.Controls.MenuItem fileOpenMenuItem = new System.Windows.Controls.MenuItem() { Header = "Open" };
            System.Windows.Controls.MenuItem fileSaveMenuItem = new System.Windows.Controls.MenuItem() { Header = "Save" };
            fileNewMenuItem.Click += fileMenuItem_Click;
            fileOpenMenuItem.Click += fileMenuItem_Click;
            fileSaveMenuItem.Click += fileMenuItem_Click;
            fileMenuItem.Items.Add(fileNewMenuItem);
            fileMenuItem.Items.Add(fileOpenMenuItem);
            fileMenuItem.Items.Add(fileSaveMenuItem);
            menu.Items.Add(fileMenuItem);

            grid.Children.Add(menu);
            if(object.ReferenceEquals(null,DocumentViewType))
            {
                System.Windows.Controls.Label label = new System.Windows.Controls.Label() { Content = "to customize DocumentView, set the DocumentViewType property to a type assignable to System.Windows.FrameworkElement." };
                grid.Children.Add(label);
                System.Windows.Controls.Grid.SetRow(label, 1);
            }
            else
            {
                documentView = (System.Windows.FrameworkElement)System.Activator.CreateInstance(DocumentViewType);
                documentView.DataContext = Document;
                grid.Children.Add(documentView);
                System.Windows.Controls.Grid.SetRow(documentView, 1);
            }
            Content = grid;
        }

        void fileMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem menuItem = sender as System.Windows.Controls.MenuItem;
            if (menuItem.Header.ToString() == "New") New();
            if (menuItem.Header.ToString() == "Open") Open();
            if (menuItem.Header.ToString() == "Save") Save();
            if (menuItem.Header.ToString() == "SaveAs") SaveAs();
        }

        void On_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            update();
        }

        private void update()
        {
            object model = Document;
            if(!object.ReferenceEquals(null,model))
            {
                if (object.ReferenceEquals(null, documentType)) documentType = model.GetType();
            }
        }

        private void New()
        {
            Document = System.Activator.CreateInstance(DocumentType);
            grid.Children.Remove(documentView);
            documentView = (System.Windows.FrameworkElement)System.Activator.CreateInstance(DocumentViewType);
            documentView.DataContext = Document;
            grid.Children.Add(documentView);
            System.Windows.Controls.Grid.SetRow(documentView, 1);
            fileName = "";
        }

        private void Open()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = OpenFileDialogFilter;
            System.Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                Document = System.Activator.CreateInstance(DocumentType);
                using(System.IO.FileStream stream = new System.IO.FileStream(ofd.FileName,System.IO.FileMode.Open))
                {
                    System.Type[] types = {typeof(System.IO.Stream)};
                    System.Reflection.MethodInfo openInfo = Document.GetType().GetMethod("Open",types);
                    if (!object.ReferenceEquals(null, openInfo))
                    {
                        object[] parameters = { stream };
                        openInfo.Invoke(Document, parameters);
                        fileName = ofd.FileName;
                    }
                }
                grid.Children.Remove(documentView);
                documentView = (System.Windows.FrameworkElement)System.Activator.CreateInstance(DocumentViewType);
                documentView.DataContext = Document;
                grid.Children.Add(documentView);
                System.Windows.Controls.Grid.SetRow(documentView, 1);
            }
        }

        private void Save()
        {
            if (fileName.Length == 0) SaveAs();
            else
            {
                Save(fileName);
            }
        }

        private void SaveAs()
        {
            Microsoft.Win32.SaveFileDialog ofd = new Microsoft.Win32.SaveFileDialog();
            ofd.Filter = OpenFileDialogFilter;
            System.Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                Save(ofd.FileName);
            }
        }

        private void Save(string filename)
        {
            using(System.IO.FileStream stream = new System.IO.FileStream(filename,System.IO.FileMode.Create))
            {
                System.Type[] types = { typeof(System.IO.Stream) };
                System.Reflection.MethodInfo saveInfo = Document.GetType().GetMethod("Save", types);
                if (!object.ReferenceEquals(null, saveInfo))
                {
                    object[] parameters = { stream };
                    saveInfo.Invoke(Document, parameters);
                    fileName = filename;
                }
            }
        }
    }
}
