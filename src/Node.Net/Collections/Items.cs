using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Node.Net.Collections
{
	public class Items<T> : ObservableCollection<T>
	{
        public Items() { }
        public Items(IEnumerable<T> items)
        {
            foreach(var item in items) { Add(item); }
            if (Count > 0) SelectedItem = this[0];
        }

		/// <summary>
		/// The currently selected item
		/// </summary>
		public T SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(SelectedItem)));
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedName)));
			}
		}

		private T _selectedItem;

        private static string GetName(T item)
        {
            if (item != null)
            {
                var nameProperty = item.GetType().GetProperty("Name");
                if (nameProperty != null)
                {
                    return nameProperty.GetValue(item).ToString();
                }
            }
            return string.Empty;
        }

		/// <summary>
		/// Names of items
		/// </summary>
		public IEnumerable<string> Names
		{
			get
			{
				var names = new List<string>();
				foreach (var item in this)
				{
                    names.Add(GetName(item));
				}
				return names;
			}
		}

		/// <summary>
		/// Name of the selected item
		/// </summary>
		public string SelectedName
		{
			get
			{
                return GetName(SelectedItem);
			}
			set
			{
                foreach(T item in this)
                {
                    if (GetName(item) == value) SelectedItem = item;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedName)));
                }
			}
		}

	}
}
