using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Beta.Internal.Collections
{
	class ReadOnlyDictionary : IDictionary
	{
		public ReadOnlyDictionary(IDictionary value) { data = value; }
		protected IDictionary data;
		public IDictionary Load(Stream stream)
		{
			data = Internal.Readers.JSONReader.Default.Read(stream) as IDictionary;
			if (data == null) data = new Dictionary<string, dynamic>();
			this.DeepUpdateParents();
			return this;
		}
		#region ICollection Members
		public bool IsSynchronized { get { return data.IsSynchronized; } }
		public object SyncRoot { get { return data.SyncRoot; } }
		public int Count { get { return data.Count; } }
		public void CopyTo(Array array, int index) { foreach (var key in data.Keys) { array.SetValue(data[key], index); ++index; } }
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator() { return ((IDictionary)this).GetEnumerator(); }
		#endregion

		#region IDictionary Members
		public virtual bool IsReadOnly { get { return true; } }
		public bool Contains(object key) { return data.Contains(key.ToString()); }
		public virtual bool IsFixedSize { get { return true; } }
		public virtual void Remove(object key) { throw new NotImplementedException(); }
		public virtual void Clear() { throw new NotImplementedException(); }
		public virtual void Add(object key, object value) { throw new NotImplementedException(); }
		public ICollection Keys { get { return data.Keys; } }
		public ICollection Values { get { return data.Values; } }
		public virtual object this[object key]
		{
			get { return data[key.ToString()]; }
			set { throw new NotImplementedException(); }
		}
		public IDictionaryEnumerator GetEnumerator() { return new ReadOnlyDictionaryEnumerator(this); }
		private class ReadOnlyDictionaryEnumerator : IDictionaryEnumerator
		{
			readonly ReadOnlyDictionary readOnlyDictionary;
			readonly Dictionary<int, string> indexedKeys;
			Int32 index = -1;

			public ReadOnlyDictionaryEnumerator(ReadOnlyDictionary value)
			{
				readOnlyDictionary = value;

				indexedKeys = new Dictionary<int, string>();
				var i = 0;
				foreach (var key in readOnlyDictionary.Keys)
				{
					indexedKeys.Add(i, key.ToString());
					++i;
				}
			}

			public Object Current { get { return new DictionaryEntry(Key, Value); } }
			public DictionaryEntry Entry { get { return (DictionaryEntry)Current; } }
			public Object Key { get { ValidateIndex(); return indexedKeys[index]; } }
			public Object Value { get { ValidateIndex(); return readOnlyDictionary[indexedKeys[index]]; } }
			public Boolean MoveNext()
			{
				if (index < indexedKeys.Count - 1) { index++; return true; }
				return false;
			}

			private void ValidateIndex()
			{
				if (index < 0 || index >= indexedKeys.Count)
					throw new InvalidOperationException("Enumerator is before or after the collection.");
			}
			public void Reset() { index = -1; }
		}
		#endregion
	}
}
