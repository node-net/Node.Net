using System;
using System.Collections;
using System.Windows;

namespace Node.Net.Framework.Controls
{
    public class SDIWindow : Window
    {
        private SDIControl sdiControl = new SDIControl();
        public SDIWindow(string name,Type documentType,string openFileDialogFilter,FrameworkElement documentView)
        {
            initialize(name, documentType, openFileDialogFilter, documentView);
        }
        public SDIWindow(string name, Type documentType, string openFileDialogFilter, FrameworkElement[] views)
        {
            initialize(name, documentType, openFileDialogFilter, new DynamicView(views));
        }
        public SDIWindow(string name, Type documentType, string openFileDialogFilter, IDictionary views)
        {
            initialize(name, documentType, openFileDialogFilter, new DynamicView(views));
        }

        private void initialize(string name, Type documentType, string openFileDialogFilter, FrameworkElement documentView)
        {
            Name = name;
            sdiControl = new SDIControl()
            {
                Documents = new Framework.Documents()
                {
                    DefaultDocumentType = documentType,
                    OpenFileDialogFilter = openFileDialogFilter
                },
                DocumentView = documentView
            };
            Content = sdiControl;
            WindowState = WindowState.Maximized;
            update();
        }


        private void update()
        {
            Title = Name;
        }

        public Documents Documents
        {
            get { return sdiControl.Documents; }
            set
            {
                if(!object.ReferenceEquals(value,sdiControl.Documents))
                {
                    if (!object.ReferenceEquals(null, sdiControl.Documents))
                    {
                        sdiControl.Documents.PropertyChanged -= Documents_PropertyChanged;
                    }
                    sdiControl.Documents = value;
                    sdiControl.Documents.PropertyChanged += Documents_PropertyChanged;
                }
                
            }
        }

        private void Documents_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="CurrentKey")
            {
                update();
            }
        }

        public FrameworkElement DocumentView
        {
            get { return sdiControl.DocumentView; }
            set { sdiControl.DocumentView = value; }
        }
    }
}
