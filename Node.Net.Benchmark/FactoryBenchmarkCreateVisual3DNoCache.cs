using NBench;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Benchmark
{
    public class FactoryBenchmarkCreateVisual3DNoCache
    {
        private Factory factoryNoCache;
        private Factory factoryWithCache;
        private IDictionary DeepModel;
        private Counter createDeepModelCounter;
        private Counter createVisual3DNoCacheCounter;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            //createDeepModelCounter = context.GetCounter("CreateDeepModelCounter");
            createVisual3DNoCacheCounter = context.GetCounter("CreateVisual3DNoCacheCounter");
            DeepModel = CreateDeepModel();
            factoryNoCache = new Factory { Cache = false };
            factoryNoCache.ManifestResourceAssemblies.Add(typeof(FactoryBenchmarkCreateVisual3DNoCache).Assembly);
            factoryWithCache = new Factory { Cache = true };
            factoryWithCache.ManifestResourceAssemblies.Add(typeof(FactoryBenchmarkCreateVisual3DNoCache).Assembly);
            var v3d = factoryWithCache.Create<Visual3D>(DeepModel);
            if (v3d == null) throw new System.Exception("v3d was null");
        }

        /*
        [PerfBenchmark(Description = "Create Deep IDictionary Model",
                       NumberOfIterations = 3, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("CreateDeepModelCounter")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void CreateDeepModel()
        {
            var model = CreateDeepModel();
            createDeepModelCounter.Increment();
        }*/

        [PerfBenchmark(Description = "Create Visual3D No Caching",
                       NumberOfIterations = 3, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("CreateVisual3DNoCacheCounter")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void CreateVisual3DNoCache()
        {
            var v3d = factoryNoCache.Create<Visual3D>(DeepModel);
            createVisual3DNoCacheCounter.Increment();
        }
        private IDictionary CreateDeepModel()
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
        private IDictionary CreateChildModel()
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
