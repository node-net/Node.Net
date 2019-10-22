using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    [TestFixture]
    class StringExtensionText
    {
        [TestCase(@"C:\temp\test.txt",true)]
        [TestCase(@"C:\??",false)]
        [TestCase("JSON Files(.json) | *.json | All Files(*.*) | *.* ",false)]
        public void String_IsValidFileName(string value,bool expected_result)
        {
            Assert.AreEqual(expected_result, value.IsValidFileName(), $"{value}");
        }

        [TestCase(@"C:\temp\test.txt", false)]
        [TestCase(@"C:\??", false)]
        [TestCase("JSON Files(.json) | *.json | All Files(*.*) | *.* ", true)]
        public void String_IsFileDialogFilter(string value, bool expected_result)
        {
            Assert.AreEqual(expected_result, value.IsFileDialogFilter(), $"{value}");
        }
    }
}
