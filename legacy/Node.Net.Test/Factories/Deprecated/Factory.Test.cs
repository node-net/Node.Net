using NUnit.Framework;
using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Node.Net.Factories.Deprecated.Factories.Generic;
using Node.Net.Factories.Deprecated.Factories.Helpers;
using System.Collections.Generic;
using static System.Math;
using System.Xml;

namespace Node.Net.Factories.Deprecated.Test
{
    [TestFixture]
    public class FactoryTest
    {


        [TestCase("Mesh.Cube.xaml")]
        [TestCase("Mesh.Square.xaml")]
        [TestCase("Model.Cube.Red.xaml")]
        [TestCase("Model.Cylinder.xaml")]
        [TestCase("Project.Office.json")]
        public void Factory_Create_Stream(string source)
        {
            var factory = new Node.Net.Factories.Deprecated.Factory(typeof(FactoryTest).Assembly);
            var stream = factory.Create<Stream>(source, null);
            Assert.IsNotNull(stream, nameof(stream));
        }

        [Test]
        [TestCase("Scene.Office.json", 10)]
        public void Factory_Create_Visual3D(string source, int count)
        {
            var factory = new Factory { GetFunction = GetHelper.GetResource };
            factory.Add("default", Factory.Default.Create<IFactory>(null, null));
            //var factory = new Factory(typeof(FactoryTest).Assembly);
            var dictionary = GlobalFixture.Read(source);
            Assert.NotNull(dictionary, nameof(dictionary));
            Visual3D v3d = null;
            for (int i = 0; i < 10; ++i)
            {
                v3d = factory.Create<Visual3D>(dictionary, null);
            }
            Assert.NotNull(v3d, nameof(v3d));
        }
        [Test]
        [TestCase(typeof(MeshGeometry3D), "Mesh.Square.xaml")]
        [TestCase(typeof(IDictionary), "Scene.Cube.json")]
        public void Factory_CreateFromStream(Type type, string resourceName)
        {
            var factory = new Factory();
            factory.ResourceAssemblies.Add(typeof(GlobalFixture).Assembly);
            /*
            var stream = GlobalFixture.GetStream(resourceName);
            Assert.NotNull(stream, $"resourceName {resourceName} produced a null stream");
            Func<Stream, object> readFunction = Node.Net.Factories.Factories.SourceFactories.StreamSourceFactory.DefaultReadFunction;
            if (resourceName.Contains(".json")) readFunction = JsonRead;
            var streamSourceFactory = new Node.Net.Factories.Factories.SourceFactories.StreamSourceFactory(readFunction, null);
            Assert.NotNull(streamSourceFactory.Create(type, stream));*/
        }


        [Test]
        [TestCase(typeof(Color), "Blue")]
        [TestCase(typeof(Material), "Blue")]
        public void Factory_Create(Type type, object source)
        {
            var factory = new Factory();
            factory.ResourceAssemblies.Add(typeof(GlobalFixture).Assembly);
            factory.Add(new FunctionAdapter3<IColor, string>(IColorHelper.FromString));
        }

        [Test]
        [TestCase(typeof(IColor), "Blue")]
        [TestCase(typeof(Material), "Blue")]
        public void Factory_Default(Type type, object source)
        {
            var factory = Factory.Default.Create<IFactory>(null, null);
            Assert.IsNotNull(factory);

            var instance = factory.Create(type, source, null);
            Assert.NotNull(instance);
            Assert.True(type.IsAssignableFrom(instance.GetType()));
        }

        [Test]
        public void Factory_Default_IMatrix3D()
        {
            var factory = Factory.Default.Create<IFactory>(null,null);
            var matrix = factory.Create<IMatrix3D>(null,null);

            var dictionary = new Dictionary<string, dynamic>();
            matrix = factory.Create<IMatrix3D>(dictionary, null);

            dictionary["X"] = "10";
            dictionary["Y"] = "20 m";
            dictionary["Z"] = "30";

            var itranslation = factory.Create<ITranslation>(dictionary, null);
            Assert.IsNotNull(itranslation, nameof(itranslation));
            Assert.AreEqual(10.0, itranslation.Translation.X);
            Assert.AreEqual(20.0, itranslation.Translation.Y);
            Assert.AreEqual(30.0, itranslation.Translation.Z);

            matrix = factory.Create<IMatrix3D>(dictionary, null);
            Assert.AreEqual(10.0, Round(matrix.Matrix3D.Transform(new Point3D(0, 0, 0)).X, 4));
            Assert.AreEqual(20.0, Round(matrix.Matrix3D.Transform(new Point3D(0, 0, 0)).Y, 4));
            Assert.AreEqual(30.0, Round(matrix.Matrix3D.Transform(new Point3D(0, 0, 0)).Z, 4));

            dictionary = new Dictionary<string, dynamic>();
            dictionary["Orientation"] = 45;
            dictionary["X"] = "10";
            dictionary["Y"] = "20 m";
            var irotations = factory.Create<IRotations>(dictionary, null);
            Assert.AreEqual(45.0, irotations.RotationsXYZ.Z);
            matrix = factory.Create<IMatrix3D>(dictionary, null);
            Assert.False(matrix.Matrix3D.IsIdentity);
            Assert.AreEqual(10.0, Round(matrix.Matrix3D.Transform(new Point3D(0, 0, 0)).X, 4));
            Assert.AreEqual(20.0, Round(matrix.Matrix3D.Transform(new Point3D(0, 0, 0)).Y, 4));

            dictionary = new Dictionary<string, dynamic>();
            dictionary["Orientation"] = "-45 deg";
            irotations = factory.Create<IRotations>(dictionary, null);
            Assert.AreEqual(-45.0, irotations.RotationsXYZ.Z);
        }

        [Test]
        public void Factory_Default_IPrimaryModel()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Type"] = "Cube";

            var factory = new Factory { GetFunction = GetHelper.GetResource };
            factory.Add("default", Factory.Default.Create<IFactory>(null, null));

            var cube = factory.Create<Model3D>("Cube", null);
            Assert.NotNull(cube, nameof(cube));

            var itypename = factory.Create<ITypeName>(dictionary, null);
            Assert.NotNull(itypename, nameof(itypename));

            var iprimaryModel = factory.Create<IPrimaryModel>(dictionary, null);
            Assert.NotNull(iprimaryModel, nameof(iprimaryModel));
            Assert.NotNull(iprimaryModel.Model3D);
        }

        [Test]
        public void Factory_Default_Visual3D()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Type"] = "Cube";

            var factory = new Factory { GetFunction = GetHelper.GetResource };
            factory.Add("default", Factory.Default.Create<IFactory>(null, null));

            var cube = factory.Create<Model3D>("Cube", null);
            Assert.NotNull(cube, nameof(cube));

            var itypename = factory.Create<ITypeName>(dictionary, null);
            Assert.NotNull(itypename, nameof(itypename));

            var iprimaryModel = factory.Create<IPrimaryModel>(dictionary, null);
            Assert.NotNull(iprimaryModel, nameof(iprimaryModel));
            Assert.NotNull(iprimaryModel.Model3D);

            var v3d = factory.Create<Visual3D>(dictionary, null);
            Assert.NotNull(v3d, nameof(v3d));
        }
        /*
        [Test]
        [TestCase(typeof(MeshGeometry3D),"Mesh.Cube.xaml")]
        public void Factory_CreateFromString(Type targetType,string value)
        {
            var factory = new Factory(typeof(FactoryTest).Assembly) { GetFunction = GetHelper.GetResource };
            var instance = factory.Create(targetType, value);
            Assert.NotNull(instance);
            Assert.True(targetType.IsAssignableFrom(value.GetType()));
        }*/

        [Test]
        public void Factory_Ancestry()
        {
            var root = new Factory();
            var factoryA = new Factory();
            root.Add(nameof(factoryA), factoryA);
            Assert.AreSame(root, factoryA.GetRootAncestor());
        }
        /*
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Factory_View3D()
        {
            var view3D = new Node.Net.Factories.Test.View3D { DataContext = GlobalFixture.GetDictionaryModels() };
            var window = new Window
            {
                Title = "Factory View3D (Create<Visual3D>) tests",
                Content = view3D
            };
            window.ShowDialog();
        }*/


        public static object ReadFunction(Stream stream)
        {
            if (stream.GetName().Contains(".json"))
            {
                var reader = new Deprecated.Internal.JsonReader();
                return reader.Read(stream);
            }
            return XamlReader.Load(stream);
        }

        [Test]
        public void Factory_List_To_Double_Array()
        {

            var list = new List<double>();
            list.Add(1.23);
            list.Add(4.56);
            var doubles = Factory.Default.Create<double[]>(list, null);
            Assert.NotNull(doubles);
            Assert.AreEqual(2, doubles.Length);

            var list2 = new List<double>();
            list2.Add(7.89);
            list2.Add(0.12);

            var lists = new List<List<double>>();
            lists.Add(list);
            lists.Add(list2);
            var doubles2 = Factory.Default.Create<double[,]>(lists, null);
            Assert.NotNull(doubles);
            Assert.AreEqual(2, doubles2.GetLength(0));
            Assert.AreEqual(2, doubles2.GetLength(1));
        }

        [Test]
        public void Factory_LocalToParent()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 m";

            var localToParent = Factory.Default.Create<ILocalToParent>(dictionary, null);
            Assert.NotNull(localToParent,nameof(localToParent));

            var parentOrigin = localToParent.LocalToParent.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(10, parentOrigin.X);
        }

        [Test]
        public void Factory_LocalToWorld()
        {
            
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 m";

            var childDictionary = new Dictionary<string, dynamic>();
            childDictionary["X"] = "1 m";

            dictionary["child"] = childDictionary;
            var child = dictionary["child"] as IDictionary;
            dictionary.UpdateParentBindings();
            Assert.AreSame(dictionary, child.GetParent(),"child.GetParent()");

            var localToWorld = Factory.Default.Create<ILocalToWorld>(child, null);
            Assert.NotNull(localToWorld, nameof(localToWorld));

            var worldOrigin = localToWorld.LocalToWorld.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(11, worldOrigin.X);
        }
        [Test]
        public void Factory_LocalToWorld_Helper()
        {

            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 m";

            var childDictionary = new Dictionary<string, dynamic>();
            childDictionary["X"] = "1 m";

            dictionary["child"] = childDictionary;
            var child = dictionary["child"] as IDictionary;
            dictionary.UpdateParentBindings();
            Assert.AreSame(dictionary, child.GetParent(), "child.GetParent()");

            var localToWorld = Node.Net.Factories.Extension.IDictionaryExtension.GetLocalToWorld(dictionary);// Factory.Default.Create<ILocalToWorld>(child);
            Assert.NotNull(localToWorld, nameof(localToWorld));

            // TODO :fix
            var worldOrigin = localToWorld.Transform(new Point3D(0, 0, 0));
            //Assert.AreEqual(11, worldOrigin.X);
        }
        [Test]
        public void Factory_Translations()
        {
            var factory = new Factory(typeof(FactoryTest).Assembly);
            var reader = new Deprecated.Internal.JsonReader();
            var widget = reader.Read(typeof(FactoryTest).Assembly.GetManifestResourceStream("Node.Net.Factories.Test.Resources.Translations.json")) as IDictionary;
            widget.UpdateParentBindings();

            //var widget = factory.Create(typeof(IDictionary), "Translations.json") as IDictionary;
            Assert.IsNotNull(widget,nameof(widget));
            var ilocalToWorld = factory.Create<ILocalToWorld>(widget, null);
            Assert.IsNotNull(ilocalToWorld);
            var worldOrigin = ilocalToWorld.LocalToWorld.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(1, worldOrigin.X, "worldOrigin.X");
            Assert.AreEqual(2, worldOrigin.Y, "worldOrigin.Y");
            Assert.AreEqual(3, worldOrigin.Z, "worldOrigin.Z");

            var foo = widget["foo"] as IDictionary;
            Assert.IsNotNull(foo);
            Assert.AreSame(widget, foo.GetParent(), "foo.GetParent()");
            ilocalToWorld = factory.Create<ILocalToWorld>(foo, null);
            worldOrigin = ilocalToWorld.LocalToWorld.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(1.1, worldOrigin.X, "worldOrigin.X");
            Assert.AreEqual(2.2, worldOrigin.Y, "worldOrigin.Y");
            Assert.AreEqual(3.3, worldOrigin.Z, "worldOrigin.Z");

            var bar = foo["bar"] as IDictionary;
            Assert.IsNotNull(bar);
            Assert.AreSame(foo, bar.GetParent(), "bar.GetParent()");
            ilocalToWorld = factory.Create<ILocalToWorld>(bar, null);
            worldOrigin = ilocalToWorld.LocalToWorld.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(1.11, Round(worldOrigin.X, 2), "worldOrigin.X");
            Assert.AreEqual(2.22, Round(worldOrigin.Y, 2), "worldOrigin.Y");
            Assert.AreEqual(3.33, Round(worldOrigin.Z, 2), "worldOrigin.Z");
        }
    }
}
