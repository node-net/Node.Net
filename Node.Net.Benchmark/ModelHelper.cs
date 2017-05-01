using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Benchmark
{
    class ModelHelper
    {
        public static IDictionary CreateDeepModel()
        {
            var model = new Dictionary<string, dynamic>();
            for (double x = 0.0; x < 10.1; x += 1.5)
            {
                for (double y = 0.0; y < 10.1; y += 1.5)
                {
                    var key = $"{x},{y}";
                    var value = new Dictionary<string, dynamic>
                    {
                        {"Type","Cube" },{"X" ,$"{x} m"},{"Y",$"{y} m" },{"child",CreateChildModel()}
                    };
                    model.Add(key, value);
                }
            }
            return model;
        }
        private static IDictionary CreateChildModel()
        {
            var model = new Dictionary<string, dynamic>
            {
                {"s0" , new Dictionary<string,dynamic>{{ "Type","Sphere" },{ "X","-0.5 m" }, {"Y","-0.5 m" },{"Z","1 m"} } },
                {"s1" ,new Dictionary<string,dynamic>{{ "Type","Sphere" },{ "X","0.5 m" }, { "Y","-0.5 m" },{ "Z","1 m"} } },
                {"s2" , new Dictionary<string,dynamic>{{ "Type","Sphere" },{ "X","0.5 m" }, {"Y","0.5 m" },{"Z","1 m"} } },
                {"s3" ,new Dictionary<string,dynamic>{{ "Type","Sphere" },{ "X","-0.5 m" }, { "Y","0.5 m" },{ "Z","1 m"} } }
            };
            return model;
        }
    }
}
