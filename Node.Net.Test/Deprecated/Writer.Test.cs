using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Node.Net.Deprecated
{
    [TestFixture]
    class WriterTest
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Writer_Save()
        {
            var data = new Dictionary<string, dynamic>();
            Deprecated.Writer.Default.Save(data);
        }
    }
}
