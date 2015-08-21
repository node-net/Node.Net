﻿using NUnit.Framework;

namespace Node.Net.Measurement
{
    [NUnit.Framework.TestFixture]
    class Length_Test
    {
        [NUnit.Framework.TestCase]
        public void Length_Usage()
        {
            Length length = Length.Parse("");
            NUnit.Framework.Assert.AreEqual(0, length[LengthUnit.Meter]);
        }
    }
}
