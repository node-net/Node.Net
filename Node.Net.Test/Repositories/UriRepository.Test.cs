using NUnit.Framework;
using System;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Node.Net.Repositories.Test
{
    class UriRepositoryTest
    {

        [Test, Apartment(ApartmentState.STA)]
        [TestCase("http://www.makoto3.net/xaml/CubeSample002.xaml", typeof(Page))]
        public void UriRepository_Get(string name, Type expectedType)
        {
            var repository = new UriRepository();
            var instance = repository.Get(name);
            Assert.NotNull(instance, nameof(instance));
            Assert.AreSame(expectedType, instance.GetType());
        }

        [Test, Apartment(ApartmentState.STA)]
        public void UriRepository_Get_FileSystem()
        {
            var filename = $"{Path.GetTempPath()}\\ListBox.Test.xaml";
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                XamlWriter.Save(new ListBox(), fs);
            }

            var repository = new UriRepository();
            var instance = repository.Get(filename);
            Assert.NotNull(instance, nameof(instance));
            Assert.AreSame(typeof(ListBox), instance.GetType());
        }
    }
}
