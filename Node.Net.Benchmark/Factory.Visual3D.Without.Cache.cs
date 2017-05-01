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
    class FactoryVisual3DWithoutCache
    {
        private Factory factoryWithoutCache;
        private IDictionary DeepModel;
        private Counter counterWithoutCache;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            counterWithoutCache = context.GetCounter("WithoutCache");
            DeepModel = ModelHelper.CreateDeepModel();
            factoryWithoutCache = new Factory { Cache = false };
            factoryWithoutCache.ManifestResourceAssemblies.Add(typeof(FactoryVisual3DWithoutCache).Assembly);
        }

        [PerfBenchmark(Description = "Create Visual3D Without Caching",
                       NumberOfIterations = 3, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("WithoutCache")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void WithoutCache()
        {
            var v3d = factoryWithoutCache.Create<Visual3D>(DeepModel);
            counterWithoutCache.Increment();
        }
    }
}
