﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Resources
{
    [TestFixture,Category(nameof(Resources))]
    class MaterialResourcesTest
    {
        [Test]
        public void MaterialResources_IsKnownColor()
        {
            Assert.False(MaterialResources.IsKnownColor("Widget"));
            Assert.True(MaterialResources.IsKnownColor("Blue"));
        }
    }
}