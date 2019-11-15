using Node.Net.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Node.Net.Collections
{
	public class List : System.Collections.Generic.List<object> , INotifyPropertyChanged
	{
		public string ToJson()
		{
			return new JsonWriter().WriteToString(this);
		}

		public static List Parse(Stream stream)
		{
			return new JsonReader { DefaultArrayType = typeof(List) }.Read(stream) as List;
		}

		public static List Parse(string json)
		{
			return Parse(new MemoryStream(Encoding.UTF8.GetBytes(json)));
		}

        public object? Parent
        {
            get { return _parent; }
            set
            {
                if (!object.ReferenceEquals(_parent, value))
                {
                    _parent = value;
                    OnPropertyChanged();
                }
            }
        }

        private object? _parent;

        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
