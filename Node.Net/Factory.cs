//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public sealed class Factory : IFactory, IDisposable
    {
        static Factory()
        {
            global::Node.Net.Factories.MetaDataMap.GetMetaDataFunction = global::Node.Net.Collections.GlobalFunctions.GetMetaDataFunction;
            global::Node.Net.Collections.GlobalFunctions.GetLocalToParentFunction = global::Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToParent;
            global::Node.Net.Collections.GlobalFunctions.GetLocalToWorldFunction = global::Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToWorld;

        }
        public Factory() { }
        public Factory(Assembly assembly)
        {
            factory.ManifestResourceFactory.Assemblies.Add(assembly);
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                //reader.Dispose();
            }
        }

        public Func<Stream, object> ReadFunction
        {
            get { return factory.ReadFunction; }
            set { factory.ReadFunction = value; }
        }


        class ConcreteLocalToParent : ILocalToParent { public Matrix3D LocalToParent { get; set; } = new Matrix3D(); }
        class ConcreteLocalToWorld : ILocalToWorld { public Matrix3D LocalToWorld { get; set; } = new Matrix3D(); }
        public object Create(Type targetType, object source)
        {
            /*
            if (targetType == typeof(ILocalToParent))
            {
                var localToParent = global::Node.Net.Factories.Deprecated.Factory.Default.Create(typeof(global::Node.Net.Factories.Deprecated.ILocalToParent), source, null) as global::Node.Net.Factories.Deprecated.ILocalToParent;
                return new ConcreteLocalToParent { LocalToParent = localToParent.LocalToParent };
            }
            if (targetType == typeof(ILocalToWorld))
            {
                var localToWorld = global::Node.Net.Factories.Deprecated.Factory.Default.Create(typeof(global::Node.Net.Factories.Deprecated.ILocalToWorld), source, null) as global::Node.Net.Factories.Deprecated.ILocalToWorld;
                return new ConcreteLocalToWorld { LocalToWorld = localToWorld.LocalToWorld };
            }*/
            var result = factory.Create(targetType, source);
            var idictionary = result as IDictionary;
            if (idictionary != null)
            {
                idictionary.DeepUpdateParents();
            }
            var element = result as Element;
            if(element != null)
            {
                element.DeepUpdateParents();
            }
            return result;
        }

        public T Create<T>()
        {
            var instance = Activator.CreateInstance<T>();
            var idictionary = instance as IDictionary;
            if (idictionary != null)
            {
                if (!idictionary.Contains("Type"))
                {
                    idictionary["Type"] = instance.GetType().Name;
                }
            }
            return (T)instance;
        }

        private global::Node.Net.Factories.StandardFactory factory = new Factories.StandardFactory
        {
            ReadFunction = new Reader().Read
        };

    }
}
