using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using NBench;
using NBench.Util;

namespace Node.Net.Benchmark
{
    class FactoryCreateVisual3DWithoutCache
    {
        private Factory factory;
        private IDictionary DeepModel;
        private Counter counter;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            counter = context.GetCounter("CreateVisual3DWithoutCache");
            DeepModel = ModelHelper.CreateDeepModel();
            factory = new Factory { Cache = false };
            factory.ManifestResourceAssemblies.Add(typeof(FactoryCreateVisual3DWithoutCache).Assembly);
            var v3d = factory.Create<Visual3D>(DeepModel);
            if (v3d == null) throw new System.Exception("v3d was null");
        }

        [PerfBenchmark(Description = "Create Visual3D No Caching",
                       NumberOfIterations = 3, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("CreateVisual3DWithoutCache")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void CreateVisual3DNoCache()
        {
            var v3d = factory.Create<Visual3D>(DeepModel);
            counter.Increment();
        }

    }
}
