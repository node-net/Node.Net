﻿//
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
            global::Node.Net.Deprecated.Factories.MetaDataMap.GetMetaDataFunction = global::Node.Net.Collections.GlobalFunctions.GetMetaDataFunction;
            global::Node.Net.Collections.GlobalFunctions.GetLocalToParentFunction = global::Node.Net.Deprecated.Factories.Helpers.IDictionaryHelper.GetLocalToParent;
            global::Node.Net.Collections.GlobalFunctions.GetLocalToWorldFunction = global::Node.Net.Deprecated.Factories.Helpers.IDictionaryHelper.GetLocalToWorld;

        }
        public Factory() { }
        public Factory(Assembly assembly)
        {
            factory.ResourceAssemblies.Add(assembly);
            //factory.ManifestResourceFactory.Assemblies.Add(assembly);
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

        private readonly global::Node.Net.Deprecated.Factories.StandardFactory factory = new Deprecated.Factories.StandardFactory
        {
            ReadFunction = new Deprecated.Reader().Read
        };
        public Dictionary<Type,Type> AbstractTypes
        {
            get { return factory.AbstractFactory; }
            set
            {
                factory.AbstractFactory.Clear();
                foreach(var key in value.Keys)
                {
                    factory.AbstractFactory.Add(key, value[key]);
                }
            }
        }

        public List<Assembly> ManifestResourceAssemblies
        {
            get { return factory.ResourceAssemblies; }
            set { factory.ResourceAssemblies = value; }
        }
    }
}
