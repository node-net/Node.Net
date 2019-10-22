using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Node.Net.Controls
{
    [SetUpFixture]
    class GlobalFixture
    {
        public static Dictionary<string, IDictionary> Projects;
        [OneTimeSetUp,Apartment(ApartmentState.STA)]
        public void OneTimeSetUp()
        {
            var reader = new Test.Internal.JsonReader();
            Projects = new Dictionary<string, IDictionary>();
            foreach (var manifestResourceName in typeof(GlobalFixture).Assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains("Node.Net.Controls.Test.Resources.Project."))
                {
                    Projects.Add(manifestResourceName.Replace("Node.Net.Controls.Test.Resources.", ""),
                        reader.Read(typeof(GlobalFixture).Assembly.GetManifestResourceStream(manifestResourceName)) as IDictionary);
                }
            }

            Factories.ImageSourceFactory.Default.AliasMap.Add("Object", "Application");
            Factories.ImageSourceFactory.Default.ResourceAssemblies.Add(typeof(GlobalFixture).Assembly);
            Factories.ImageSourceFactory.Default.AliasMap.Add("Book", "Node.Net.Controls.Test.Resources.Book.png");
            //var bookImageSource = Factory.Default.Create<ImageSource>(typeof(GlobalFixture).Assembly.GetManifestResourceStream("Node.Net.Controls.Test.Resources.Book.png"));
            //Assert.NotNull(bookImageSource);
            //Factories.ImageSourceFactory.Default.ImageSourceMap.Add("Book", bookImageSource);
            //var imageSource = Factories.ImageSourceFactory.Default.Create<ImageSource>("Question").Clone();
            //Factories.ImageSourceFactory.Default.ImageSourceMap.Add("Object", imageSource);
            //Factories.ImageSourceFactory.Default.ImageSourceMap.Add("Object", Factories.ImageSourceFactory.Default.Create<ImageSource>("Question"));
        }

        public static IDictionary GetSampleDictionary()
        {
            var d = new Dictionary<string, dynamic>();
            d["Name"] = "SampleDictionary";
            d["Rank"] = 8;
            d["Score"] = 95.5;

            var c = new Dictionary<string, dynamic>();
            c["Name"] = "Child";
            c["Rank"] = 5;
            c["Score"] = 82.4;

            d["child"] = c;
            return d;
        }

        public static object Read(string name)
        {
            var assembly = typeof(GlobalFixture).Assembly;
            foreach (var manifestResourceName in assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(name))
                {
                    return Read(assembly.GetManifestResourceStream(manifestResourceName), manifestResourceName);
                }
            }
            return null;
        }

        public static object Read(Stream stream, string name)
        {
            if (name.Contains(".json"))
            {
                var reader = new Node.Net.Controls.Test.Internal.JsonReader();
                return reader.Read(stream);
            }
            return XamlReader.Load(stream);
        }
    }
}
