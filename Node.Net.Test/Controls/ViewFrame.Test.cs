using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Node.Net.Controls.Test
{
    [TestFixture]
    class ViewFrameTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void ViewFrame_Usage_0()
        {
            var viewFrame = new Node.Net.Controls.ViewFrame
            {
                DataContext = new KeyValuePair<string, dynamic>("SampleDictionary", GlobalFixture.GetSampleDictionary())
            };
            var w = new Window
            {
                Title = "ViewFrame_Usage_0",
                Content = viewFrame,
                WindowState = WindowState.Maximized
            };
            w.ShowDialog();
        }

        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void ViewFrame_Usage_1()
        {
            var viewFrame = new Node.Net.Controls.ViewFrame();
            viewFrame.Views.Add("TreeView", new Node.Net.Controls.TreeView());
            viewFrame.DataContext = new KeyValuePair<string, dynamic>("SampleDictionary", GlobalFixture.GetSampleDictionary());
            var w = new Window
            {
                Title = "ViewFrame_Usage_1",
                Content = viewFrame,
                WindowState = WindowState.Maximized
            };
            w.ShowDialog();
        }

        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void ViewFrame_Usage_2()
        {
            var viewFrame = new Node.Net.Controls.ViewFrame();
            viewFrame.Views.Add("TreeView", new Node.Net.Controls.TreeView());
            viewFrame.Views.Add("Properties", new Node.Net.Controls.Properties());
            viewFrame.DataContext = new KeyValuePair<string, dynamic>("SampleDictionary", GlobalFixture.GetSampleDictionary());
            var w = new Window
            {
                Title = "ViewFrame_Usage_2",
                Content = viewFrame,
                WindowState = WindowState.Maximized
            };
            w.ShowDialog();
        }
    }
}
