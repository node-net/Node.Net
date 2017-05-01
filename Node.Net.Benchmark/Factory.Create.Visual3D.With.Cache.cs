using NBench;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Benchmark
{
    class FactoryCreateVisual3DWithCache
    {
        private Factory factoryWithCache;
        private IDictionary DeepModel;
        private Counter counter;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            counter = context.GetCounter("CreateVisual3DWithCache");
            DeepModel = ModelHelper.CreateDeepModel();
            factoryWithCache = new Factory { Cache = true };
            factoryWithCache.ManifestResourceAssemblies.Add(typeof(FactoryBenchmarkCreateVisual3DNoCache).Assembly);
            var v3d = factoryWithCache.Create<Visual3D>(DeepModel);
            if (v3d == null) throw new System.Exception("v3d was null");
        }

        [PerfBenchmark(Description = "Create Visual3D No Caching",
                       NumberOfIterations = 3, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("CreateVisual3DWithCache")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void CreateVisual3DNoCache()
        {
            var v3d = factoryWithCache.Create<Visual3D>(DeepModel);
            counter.Increment();
        }
        
    }
}
