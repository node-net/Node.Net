using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public class Element : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public IDictionary Model
        {
            get
            {
                if (model == null) model = new Dictionary<string, dynamic>();
                return model;
            }
            set
            {
                if(model != value)
                {
                    model = value;
                    if (model != null) model.DeepUpdateParents();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Model)));
                    itemsSource = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsSource)));
                }
            }
        }
        private IDictionary model = new Dictionary<string, dynamic>();
        public string Key
        {
            get
            {
                var key = Model.GetKey();
                if (key != null) return key.ToString();
                return "?";
            }
        }
        public override string ToString()
        {
            return $"{Key}";
        }
        public bool HasKey(string name) => Model.Contains(name);
        public object Get(string name) => Model.Get<object>(name);
        public T Get<T>(string name) => Model.Get<T>(name);
        public void Set(string name, object value)
        {
            var hadKey = HasKey(name);
            if (Get(name) != value)
            {
                Model.Set(name, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                if(value != null)
                {
                    if(value.GetType() != typeof(string))
                    {
                        if(typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                        {
                            itemsSource = null;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                        }
                    }
                }
            }
        }
        public void Remove(string name)
        {
            if(HasKey(name))
            {
                var value = Get(name);
                Model.Remove(name);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                if (value != null)
                {
                    if (value.GetType() != typeof(string))
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                        {
                            itemsSource = null;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                        }
                    }
                }
            }
        }

        public ObservableCollection<Element> ItemsSource
        {
            get
            {
                if(itemsSource == null)
                {
                    itemsSource = new ObservableCollection<Element>();
                    foreach (string key in Model.Keys)
                    {
                        var child = Model[key] as IDictionary;
                        if (child != null)
                        {
                            itemsSource.Add(new Element { Model = child });
                        }
                    }
                }
                return itemsSource;
            }
        }
        private ObservableCollection<Element> itemsSource;
    }
}
