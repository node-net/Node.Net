using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Node.Net.Deprecated.Collections
{
    public class Document : Dictionary
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<T>([CallerMemberName]string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileName)));
                }
            }
        }
        private string fileName = string.Empty;
    }
}
