# Node.Net.Benchmark.FactoryVisual3DWithoutCache+WithoutCache
__Create Visual3D Without Caching__
_5/1/2017 5:30:13 PM_
### System Info
```ini
NBench=NBench, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
OS=Microsoft Windows NT 6.2.9200.0
ProcessorCount=8
CLR=4.0.30319.42000,IsMono=False,MaxGcGeneration=2
```

### NBench Settings
```ini
RunMode=Throughput, TestMode=Measurement
NumberOfIterations=3, MaximumRunTime=00:00:01
Concurrent=False
Tracing=False
```

## Data
-------------------

### Totals
|          Metric |           Units |             Max |         Average |             Min |          StdDev |
|---------------- |---------------- |---------------- |---------------- |---------------- |---------------- |
|TotalBytesAllocated |           bytes |   10,723,808.00 |    8,885,818.67 |    6,381,632.00 |    2,246,438.80 |
|[Counter] WithoutCache |      operations |            2.00 |            2.00 |            2.00 |            0.00 |

### Per-second Totals
|          Metric |       Units / s |         Max / s |     Average / s |         Min / s |      StdDev / s |
|---------------- |---------------- |---------------- |---------------- |---------------- |---------------- |
|TotalBytesAllocated |           bytes |    9,840,031.53 |    8,428,671.85 |    6,140,306.06 |    1,999,711.71 |
|[Counter] WithoutCache |      operations |            1.95 |            1.90 |            1.84 |            0.06 |

### Raw Data
#### TotalBytesAllocated
|           Run # |           bytes |       bytes / s |      ns / bytes |
|---------------- |---------------- |---------------- |---------------- |
|               1 |   10,723,808.00 |    9,840,031.53 |          101.63 |
|               2 |    6,381,632.00 |    6,140,306.06 |          162.86 |
|               3 |    9,552,016.00 |    9,305,677.96 |          107.46 |

#### [Counter] WithoutCache
|           Run # |      operations |  operations / s | ns / operations |
|---------------- |---------------- |---------------- |---------------- |
|               1 |            2.00 |            1.84 |  544,907,197.23 |
|               2 |            2.00 |            1.92 |  519,650,969.90 |
|               3 |            2.00 |            1.95 |  513,235,899.82 |


