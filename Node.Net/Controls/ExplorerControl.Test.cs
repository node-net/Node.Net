using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows;

namespace Node.Net.Controls
{
    [TestFixture,Category("Node.Net.Controls.ExplorerControl")]
    class ExplorerControlTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA),Category("Explict")]
        public void ExplorerControl_ShowDialog()
        {
            Stream stream = IO.StreamExtension.GetStream("Dictionary.Test.Positional.Scene.json");
            IDictionary dictionary = Json.Reader.Default.Read(stream) as IDictionary;
            Window w = new Window()
            {
                Title = "ExplorerControl_ShowDialog",
                Content = new ExplorerControl()
                {
                    DataContext = dictionary,
                    ContentView = new Controls.ReadOnlyTextBox()
                }
            };
            w.ShowDialog();
        }
    }
}
