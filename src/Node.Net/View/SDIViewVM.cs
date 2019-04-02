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
using System.Windows;
using System.IO;
using System.Windows.Input;

namespace Node.Net.View
{
	public class SDIViewVM : INotifyPropertyChanged
	{
		#region PersistentData
		private void SavePersistentData()
		{
			new Writer().Write(FileName, _persistentData);
		}

		private void LoadPersistentData()
		{
			_persistentData = new Reader().Read<Dictionary<string, object>>(FileName);
            OnPropertyChanged(nameof(RecentFileNames));
		}

		private Dictionary<string, object> _persistentData = new Dictionary<string, object>()
		{
			{ nameof(RecentFileNames),new List<string>() },
			{nameof(RecentFileName),string.Empty }
		};
		#endregion

		#region FileName
		public string FileName
		{
			get { return _filename; }
			set
			{
				_filename = value;
				OnPropertyChanged();
				if(File.Exists(_filename))
				{
					LoadPersistentData();
				}
			}
		}

		private string _filename;
		#endregion

		#region Document
		public object Document
		{
			get { return _document; }
			set
			{
				_document = value;
				OnPropertyChanged();
			}
		}

		private object _document;
		#endregion

		#region Open

		/// <summary>
		/// Open project from file
		/// </summary>
		/// <param name="filename"></param>
		public object Open(string filename)
		{
			using (var stream = new FileStream(filename, FileMode.Open))
			{
				Document = Open(stream);
                var recent = new List<string>(RecentFileNames);
				if (recent.Contains(filename))
				{
                    recent.Remove(filename);
				}
                recent.Insert(0, filename);
                RecentFileNames = recent;
                SavePersistentData();
				return Document;
			}
		}

		/// <summary>
		/// Open project from stream
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public virtual object Open(Stream stream)
		{
			var reader = new Reader() { DefaultDocumentType = typeof(Collections.Dictionary) };
			Document = reader.Read(stream);
			return Document;
		}

		/// <summary>
		/// Browse for project file to open
		/// </summary>
		public object Open()
		{
			var ofd = new Microsoft.Win32.OpenFileDialog
			{
				Filter = OpenFileDialogFilter,
				Multiselect = false,
				InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
			};

			var result = ofd.ShowDialog();
			if (result == true)
			{
				Open(ofd.FileName);
			}
			return Document;
		}

		/// <summary>
		/// OpenFileDialogFilter
		/// </summary>
		public string OpenFileDialogFilter
		{
			get { return _openFileDialogFilter; }
			set
			{
				_openFileDialogFilter = value;
				OnPropertyChanged();
			}
		}

		private string _openFileDialogFilter = "All Files (*.*)|*.*";

		/// <summary>
		/// ICommand to browse for file to open
		/// </summary>
		public ICommand OpenCommand { get { return new DelegateCommand(_ => Open(), null); } }

		#endregion Open

		#region RecentFileNames
		/// <summary>
		/// Recent FileNames
		/// </summary>
		public IEnumerable<string> RecentFileNames
		{
			get
			{
				return _persistentData[nameof(RecentFileNames)] as IEnumerable<string>;
			}
			set
			{
				_persistentData[nameof(RecentFileNames)] = value;
				SavePersistentData();
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Recent FileName
		/// </summary>
		public string RecentFileName
		{
			get
			{
				return _persistentData[nameof(RecentFileName)].ToString();
			}
			set
			{
				_persistentData[nameof(RecentFileName)] = value;
				SavePersistentData();
				OnPropertyChanged();
				Open(value);
			}
		}
		#endregion

		#region Views
		/// <summary>
		/// Views
		/// </summary>
		public Collections.Items<FrameworkElement> Views
		{
			get { return _views; }
			set
			{
				_views = value;
				OnPropertyChanged();
			}
		}

		private Collections.Items<FrameworkElement> _views = new Collections.Items<FrameworkElement>();
		#endregion

		#region PropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName]string caller = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
		}
		#endregion
	}
}
