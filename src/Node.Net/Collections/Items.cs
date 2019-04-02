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

		/// <summary>
		/// Names of items
		/// </summary>
		public IEnumerable<string> Names
		{
			get
			{
				var names = new List<string>();
				var nameProperty = typeof(T).GetProperty("Name");
				if (nameProperty != null)
				{
					foreach (var item in this)
					{
						names.Add(nameProperty.GetValue(item).ToString());
					}
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
				if (SelectedItem != null)
				{
					var nameProperty = SelectedItem.GetType().GetProperty("Name");
					if (nameProperty != null)
					{
						return nameProperty.GetValue(SelectedItem).ToString();
					}
				}
				return string.Empty;
			}
			set
			{
				var nameProperty = typeof(T).GetProperty("Name");
				if (nameProperty != null)
				{
					foreach (T item in this)
					{
						var cur_name = nameProperty.GetValue(item).ToString();
						if (cur_name == value)
						{
							SelectedItem = item;
							break;
						}
					}
				}
			}
		}

	}
}
