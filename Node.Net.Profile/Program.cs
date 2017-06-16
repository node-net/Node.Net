using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Profile
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Node.Net.IDictionaryExtensionTest().IDictionary_Extension_Profile_GetLocalToWorld();
            //new Node.Net.FactoryTest().Factory_Visual3D_Profile();

            /*
            var test = new Node.Net.FactoryCreateIDictionaryTest();
            test.SetUp();
            test.CreateFromManifestResourceStream("Scene.24500.json");
            */

            var test = new Node.Net.ReaderTest();
            test.Read_StressTest();
    }
    }
}
