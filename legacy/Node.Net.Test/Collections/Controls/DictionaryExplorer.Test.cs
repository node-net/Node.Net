using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Node.Net.Collections.Test.Controls
{
    [TestFixture]
    class DictionaryExplorerTest : Fixture
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void DictionaryExplorer_Usage()
        {
            var window = new Window
            {
                Title = "dictionaryExplorer_Usage",
                Content = new Node.Net.Collections.Controls.DictionaryExplorer { DataContext = new KeyValuePair<string, dynamic>("States.json", Read("States.json")) },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }
    }
}
