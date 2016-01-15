using System;
using System.Windows;

namespace Node.Net.Framework.Controls
{
    public class SDIWindow : Window
    {
        private SDIControl sdiControl = new SDIControl();
        public SDIWindow(string name,Type documentType,string openFileDialogFilter,FrameworkElement documentView)
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
        /*
        SDIWindow window = new SDIWindow("SDIWindow_Usage_Text_SingleView")
        {
            Content = new SDIControl()
            {
                Documents = new Framework.Documents()
                {
                    DefaultDocumentType = typeof(Node.Net.Documents.TextDocument),
                    OpenFileDialogFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                },
                DocumentView = new Node.Net.View.ReadOnlyTextView()
            },
        };
        SDIWindow window = new SDIWindow("SDIWindow_Usage_Text_SingleView",
                                           typeof(Node.Net.Documents.TextDocument),
                                           "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                                           new Node.Net.View.ReadOnlyTextView);
                                           */

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
