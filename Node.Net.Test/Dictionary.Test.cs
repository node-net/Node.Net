using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Automation.Peers;
using System.Windows.Media.Media3D;
using NUnit.Framework;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    class DictionaryTest
    {
        [Test]
        public void RotateOTS()
        {
            Dictionary<string, object> element = new Dictionary<string, object>();
            Assert.AreEqual(0.0, Round(element.GetLocalToWorld().GetOrientation(), 4), "orientation");
            Assert.AreEqual(0.0, Round(element.GetLocalToWorld().GetTilt(), 3), "tilt");
            Assert.AreEqual(0.0, Round(element.GetLocalToWorld().GetSpin(), 3), "spin");

            element.SetRotationsOTS(new Vector3D(55, 15, 0));
            Assert.AreEqual(55.0, Round(element.GetLocalToWorld().GetOrientation(), 4), "orientation");
            Assert.AreEqual(15.0, Round(element.GetLocalToWorld().GetTilt(), 3), "tilt");
            Assert.AreEqual(0.0, Round(element.GetLocalToWorld().GetSpin(), 3), "spin");

            Dictionary<string, object> group = new Dictionary<string, object>();
            group.Add("child", element);
            element.SetParent(group);

            element.SetRotationsOTS(new Vector3D(85.0, 32.0, 0));
            Assert.AreEqual(85.0, Round(element.GetLocalToWorld().GetOrientation(), 4), "orientation");
            Assert.AreEqual(32.0, Round(element.GetLocalToWorld().GetTilt(), 3), "tilt");
            Assert.AreEqual(0.0, Round(element.GetLocalToWorld().GetSpin(), 3), "spin");

            element["XDirection"] = "1,0,0";
            element["YDirection"] = "0,0.573462,0.819232";
            element.SetRotationsOTS(new Vector3D(15.0, 23, 0));
            Assert.AreEqual(15.0, Round(element.GetLocalToWorld().GetOrientation(), 4), "orientation");
            Assert.AreEqual(23.0, Round(element.GetLocalToWorld().GetTilt(), 3), "tilt");
            Assert.AreEqual(0.0, Round(element.GetLocalToWorld().GetSpin(), 3), "spin");
        }
    }
}
