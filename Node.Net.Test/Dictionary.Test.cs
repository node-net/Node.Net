#if IS_WINDOWS
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal class DictionaryTest
    {
        [Test]
        public void RotateOTS()
        {
            Dictionary<string, object> element = new Dictionary<string, object>();
            Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4),Is.EqualTo(0.0), "orientation");
            Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3),Is.EqualTo(0.0), "tilt");
            Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3),Is.EqualTo(0.0), "spin");

            element.SetRotationsOTS(new Vector3D(55, 15, 0));
            Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4), Is.EqualTo(55.0),"orientation");
            Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3), Is.EqualTo(15.0), "tilt");
            Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3), Is.EqualTo(0.0), "spin");

            Dictionary<string, object> group = new Dictionary<string, object>();
            group.Add("child", element);
            element.SetParent(group);

            element.SetRotationsOTS(new Vector3D(85.0, 32.0, 0));
            Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4), Is.EqualTo(85.0), "orientation");
            Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3), Is.EqualTo(32.0), "tilt");
            Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3), Is.EqualTo(0.0), "spin");

            element["XDirection"] = "1,0,0";
            element["YDirection"] = "0,0.573462,0.819232";
            element.SetRotationsOTS(new Vector3D(15.0, 23, 0));
            Assert.That(Round(element.GetLocalToWorld().GetOrientation(), 4), Is.EqualTo(15.0), "orientation");
            Assert.That(Round(element.GetLocalToWorld().GetTilt(), 3), Is.EqualTo(23.0), "tilt");
            Assert.That(Round(element.GetLocalToWorld().GetSpin(), 3), Is.EqualTo(0.0), "spin");
        }
    }
}
#endif
