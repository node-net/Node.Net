﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Node.Net.Collections
{
    [Serializable]
    public class Dictionary : Dictionary<string, object>, ISerializable, INotifyPropertyChanged
    {
        #region Construction

        public Dictionary()
        {
        }

        #endregion Construction

        public string ToJson { get { return (this as IDictionary).ToJson(); } set { } }

        #region Serialization

        protected Dictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion Serialization

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

        #endregion PropertyChanged

        public static Dictionary? Parse(Stream stream)
        {
            return new Reader
            {
                DefaultDocumentType = typeof(Dictionary),
                DefaultObjectType = typeof(Dictionary)
            }.Read(stream) as Dictionary;
        }

        public static Dictionary? Parse(string json)
        {
            return Parse(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }
    }
}