using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public class Document : Dictionary
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<T>([CallerMemberName]string caller=null)
        {
            var handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
        public string FileName
        {
            get { return fileName; }
            set
            {
                if(fileName != value)
                {
                    fileName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileName)));
                }
            }
        }
        private string fileName = string.Empty;
    }
}
