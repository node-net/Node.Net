using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public class GlobalFixture
    {
        /*
        public static object Read(string name)
        {
            return Node.Net.Readers.JsonReader.Default.Read(GetStream(name));
        }*/
        public static void StressTest()
        {
            var test = new IDictionaryExtensionTest();
            //test.IDictionaryExtension_DeepCollect_Stress_TypeName();
            //test.IDictionaryExtension_DeepCollect_Stress();
            //test.IDictionaryExtension_DeepUpdateParents_Stress();
        }
    }
}
