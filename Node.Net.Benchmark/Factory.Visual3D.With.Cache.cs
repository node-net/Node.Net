using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using NBench;

namespace Node.Net.Benchmark
{
    class FactoryVisual3DWithCache
    {
        private Factory factoryWithCache;
        private IDictionary DeepModel;
        private Counter counterWithCache;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            counterWithCache = context.GetCounter("WithCache");
            DeepModel = ModelHelper.CreateDeepModel();
            factoryWithCache = new Factory { Cache = true };
            factoryWithCache.ManifestResourceAssemblies.Add(typeof(FactoryVisual3DWithCache).Assembly);
            var v3d = factoryWithCache.Create<Visual3D>(DeepModel);
            if (v3d == null) throw new System.Exception("v3d was null");
        }

        [PerfBenchmark(Description = "Create Visual3D With Caching",
                       NumberOfIterations = 3, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("WithCache")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void WithCache()
        {
            var v3d = factoryWithCache.Create<Visual3D>(DeepModel);
            counterWithCache.Increment();
        }


    }
}
