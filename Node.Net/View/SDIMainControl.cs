namespace Node.Net.View
{
    public class SDIMainControl : System.Windows.Controls.UserControl
    {
        public SDIMainControl(System.Type docType, System.Windows.FrameworkElement docView)
        {
            DocumentType = docType;
            documentView = docView;
            DataContextChanged += SDIMainForm_DataContextChanged;
            New();
        }

        public bool? ShowDialog()
        {
            System.Windows.Window window = new System.Windows.Window()
            {
                Content = this,
                Title = this.Title,
                WindowState = System.Windows.WindowState.Maximized
            };
            return window.ShowDialog();
        }
        private string applicationName = "SDIPad";
        public string ApplicationName
        {
            get { return applicationName; }
            set
            {
                applicationName = value;
                Update();
            }
        }

        void SDIMainForm_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private System.Type documentType = null;
        public System.Type DocumentType
        {
            get { return documentType; }
            set { documentType = value; }
        }

        public object Document
        {
            get
            {
                object document = null;
                System.Collections.IEnumerable ienumerable = DataContext as System.Collections.IEnumerable;
                if (!object.ReferenceEquals(null, ienumerable))
                {
                    foreach (object item in ienumerable) document = item;
                    return document;
                }
                return KeyValuePair.GetValue(DataContext);
            }
            set
            {

                System.Collections.IEnumerable ienumerable = DataContext as System.Collections.IEnumerable;
                if (!object.ReferenceEquals(null, ienumerable))
                {
                    System.Collections.IList ilist = ienumerable as System.Collections.IList;
                    if (object.ReferenceEquals(null, ilist))
                    {
                        System.Collections.Generic.List<object> list
                            = new System.Collections.Generic.List<object>();
                        foreach (object item in ilist) list.Add(item);
                        list[list.Count - 1] = value;
                    }
                    else
                    {
                        ilist[ilist.Count - 1] = value;
                    }
                    DataContext = null;
                    DataContext = ilist;
                }
                else
                {
                    DataContext = null;
                    DataContext = value;
                }
                Update();
            }
        }
        private string fileName = "";
        private System.Windows.Controls.Grid documentViewGrid = new System.Windows.Controls.Grid();
        private System.Windows.FrameworkElement documentView = null;

        private string openFileDialogFilter = "Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        public string OpenFileDialogFilter
        {
            get { return openFileDialogFilter; }
            set { openFileDialogFilter = value; }
        }

        private string saveFileDialogFilter = "Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        public string SaveFileDialogFilter
        {
            get { return saveFileDialogFilter; }
            set { saveFileDialogFilter = value; }
        }
        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = System.Windows.GridLength.Auto });
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());

            System.Windows.Controls.Menu menu = new System.Windows.Controls.Menu();
            System.Windows.Controls.MenuItem fileMenuItem = new System.Windows.Controls.MenuItem() { Header = "File" };
            System.Windows.Controls.MenuItem fileNewMenuItem = new System.Windows.Controls.MenuItem() { Header = "New" };
            System.Windows.Controls.MenuItem fileOpenMenuItem = new System.Windows.Controls.MenuItem() { Header = "Open" };
            System.Windows.Controls.MenuItem fileSaveMenuItem = new System.Windows.Controls.MenuItem() { Header = "Save" };
            System.Windows.Controls.MenuItem fileSaveAsMenuItem = new System.Windows.Controls.MenuItem() { Header = "SaveAs" };
            System.Windows.Controls.MenuItem fileExitMenuItem = new System.Windows.Controls.MenuItem() { Header = "Exit" };
            fileNewMenuItem.Click += fileMenuItem_Click;
            fileOpenMenuItem.Click += fileMenuItem_Click;
            fileSaveMenuItem.Click += fileMenuItem_Click;
            fileSaveAsMenuItem.Click += fileMenuItem_Click;
            fileExitMenuItem.Click += fileMenuItem_Click;
            fileMenuItem.Items.Add(fileNewMenuItem);
            fileMenuItem.Items.Add(fileOpenMenuItem);
            fileMenuItem.Items.Add(fileSaveMenuItem);
            fileMenuItem.Items.Add(fileSaveAsMenuItem);
            fileMenuItem.Items.Add(fileExitMenuItem);
            menu.Items.Add(fileMenuItem);

            grid.Children.Add(menu);
            grid.Children.Add(documentViewGrid);
            System.Windows.Controls.Grid.SetRow(documentViewGrid, 1);
            Content = grid;
        }

        private string title = "";
        public string Title
        {
            get{return title;}
            set{title=value;}
        }
        public void Update()
        {
            object model = KeyValuePair.GetValue(DataContext);
            if (!object.ReferenceEquals(null, model))
            {
                if (object.ReferenceEquals(null, documentType)) documentType = model.GetType();
            }
            string title = "";
            object key = KeyValuePair.GetKey(Document);
            if (!object.ReferenceEquals(null, key)) title = key.ToString() + " - ";
            title += ApplicationName;
            Title = title;

            object doc = KeyValuePair.GetValue(Document);
            if (object.ReferenceEquals(null, doc) || doc.GetType() != DocumentType) New();
            if (!documentViewGrid.Children.Contains(documentView))
            {
                documentViewGrid.Children.Clear();
                documentViewGrid.Children.Add(documentView);
            }
            documentView.DataContext = DataContext;
        }

        void fileMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem menuItem = sender as System.Windows.Controls.MenuItem;
            if (menuItem.Header.ToString() == "New") New();
            if (menuItem.Header.ToString() == "Open") Open();
            if (menuItem.Header.ToString() == "Save") Save();
            if (menuItem.Header.ToString() == "SaveAs") SaveAs();
            if (menuItem.Header.ToString() == "Exit") Exit();
        }
        private void New()
        {
            fileName = "";
            Document = new System.Collections.Generic.KeyValuePair<string, object>("Untitled", System.Activator.CreateInstance(DocumentType));
            //DataContext = new System.Collections.Generic.KeyValuePair<string, object>("Untitled", System.Activator.CreateInstance(DocumentType));
        }
        private void Open()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = OpenFileDialogFilter;
            System.Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                object document = System.Activator.CreateInstance(DocumentType);

                System.Type[] stringTypes = { typeof(string) };
                System.Reflection.MethodInfo openStringInfo = document.GetType().GetMethod("Open", stringTypes);
                if (!object.ReferenceEquals(null, openStringInfo))
                {
                    object[] parameters = { ofd.FileName };
                    try
                    {
                        openStringInfo.Invoke(document, parameters);
                        fileName = ofd.FileName;
                    }
                    catch (System.Exception exception)
                    {
                        System.Windows.MessageBox.Show(exception.ToString());
                        New();
                        return;
                    }
                }
                else
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open))
                    {
                        System.Type[] types = { typeof(System.IO.Stream) };
                        System.Reflection.MethodInfo openInfo = document.GetType().GetMethod("Open", types);
                        if (!object.ReferenceEquals(null, openInfo))
                        {
                            object[] parameters = { stream };
                            try
                            {
                                openInfo.Invoke(document, parameters);
                                fileName = ofd.FileName;
                            }
                            catch (System.Exception exception)
                            {
                                System.Windows.MessageBox.Show(exception.ToString());
                                New();
                                return;
                            }
                        }
                    }
                }
                System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                Document = new System.Collections.Generic.KeyValuePair<string, object>
                                   (fi.Name, document);
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
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = SaveFileDialogFilter;
            System.Nullable<bool> result = sfd.ShowDialog();
            if (result == true)
            {
                Save(sfd.FileName);
            }
        }

        private void Save(string filename)
        {
            using (System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Create))
            {
                object document = KeyValuePair.GetValue(Document);
                System.Type[] types = { typeof(System.IO.Stream) };
                System.Reflection.MethodInfo saveInfo = document.GetType().GetMethod("Save", types);
                if (!object.ReferenceEquals(null, saveInfo))
                {
                    object[] parameters = { stream };
                    saveInfo.Invoke(document, parameters);
                    fileName = filename;
                    System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                    Document = new System.Collections.Generic.KeyValuePair<string, object>
                                       (fi.Name, document);
                }
            }
        }

        private void Exit() 
        {
            System.Windows.Window mainWindow = System.Windows.Application.Current.MainWindow;
            mainWindow.Close(); 
        }
    }
}