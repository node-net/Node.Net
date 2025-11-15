using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Windows
{
#if !IS_WINDOWS
    /// <summary>
    /// Provides a dictionary that contains resources used by components and other elements of a WPF application.
    /// </summary>
    public class ResourceDictionary : IDictionary, ICollection, IEnumerable
    {
        private readonly Dictionary<object, object> _dictionary = new Dictionary<object, object>();
        private Collection<ResourceDictionary>? _mergedDictionaries;

        /// <summary>
        /// Gets or sets a collection of the ResourceDictionary dictionaries that constitute the various resource dictionaries in the merged dictionaries.
        /// </summary>
        public Collection<ResourceDictionary> MergedDictionaries
        {
            get
            {
                if (_mergedDictionaries == null)
                {
                    _mergedDictionaries = new Collection<ResourceDictionary>();
                }
                return _mergedDictionaries;
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        public object? this[object key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                if (_dictionary.TryGetValue(key, out object? value))
                {
                    return value;
                }
                // Check merged dictionaries
                if (_mergedDictionaries != null)
                {
                    foreach (var mergedDict in _mergedDictionaries)
                    {
                        if (mergedDict.Contains(key))
                        {
                            return mergedDict[key];
                        }
                    }
                }
                return null;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                if (value == null)
                {
                    _dictionary.Remove(key);
                }
                else
                {
                    _dictionary[key] = value;
                }
            }
        }

        /// <summary>
        /// Gets a collection containing the keys in the ResourceDictionary.
        /// </summary>
        public ICollection Keys => _dictionary.Keys;

        /// <summary>
        /// Gets a collection containing the values in the ResourceDictionary.
        /// </summary>
        public ICollection Values => _dictionary.Values;

        /// <summary>
        /// Gets the number of key/value pairs contained in the ResourceDictionary.
        /// </summary>
        public int Count => _dictionary.Count;

        /// <summary>
        /// Gets a value indicating whether the ResourceDictionary is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets a value indicating whether the ResourceDictionary has a fixed size.
        /// </summary>
        public bool IsFixedSize => false;

        /// <summary>
        /// Gets a value indicating whether access to the ResourceDictionary is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized => false;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the ResourceDictionary.
        /// </summary>
        public object SyncRoot => ((ICollection)_dictionary).SyncRoot;

        /// <summary>
        /// Adds a key/value pair to the ResourceDictionary.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _dictionary.Add(key, value);
        }

        /// <summary>
        /// Removes all keys and values from the ResourceDictionary.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Determines whether the ResourceDictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the ResourceDictionary.</param>
        /// <returns>true if the ResourceDictionary contains an element with the specified key; otherwise, false.</returns>
        public bool Contains(object key)
        {
            if (key == null)
            {
                return false;
            }
            if (_dictionary.ContainsKey(key))
            {
                return true;
            }
            // Check merged dictionaries
            if (_mergedDictionaries != null)
            {
                foreach (var mergedDict in _mergedDictionaries)
                {
                    if (mergedDict.Contains(key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Removes the element with the specified key from the ResourceDictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public void Remove(object key)
        {
            if (key == null)
            {
                return;
            }
            _dictionary.Remove(key);
        }

        /// <summary>
        /// Copies the elements of the ResourceDictionary to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ResourceDictionary.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            ((ICollection)_dictionary).CopyTo(array, index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the ResourceDictionary.
        /// </summary>
        /// <returns>An IDictionaryEnumerator object for the ResourceDictionary.</returns>
        public IDictionaryEnumerator GetEnumerator()
        {
            return new DictionaryEnumerator(_dictionary.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<KeyValuePair<object, object>> _enumerator;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<object, object>> enumerator)
            {
                _enumerator = enumerator;
            }

            public DictionaryEntry Entry => new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value);

            public object Key => _enumerator.Current.Key;

            public object? Value => _enumerator.Current.Value;

            public object Current => Entry;

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }

        // IDictionary implementation
        void IDictionary.Add(object key, object value)
        {
            Add(key, value);
        }

        bool IDictionary.Contains(object key)
        {
            return Contains(key);
        }

        void IDictionary.Remove(object key)
        {
            Remove(key);
        }
    }
#endif
}

