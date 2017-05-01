# Node.Net.Benchmark.FactoryVisual3DWithCache+WithCache
__Create Visual3D With Caching__
_5/1/2017 5:29:55 PM_
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
|TotalBytesAllocated |           bytes |    1,924,928.00 |    1,924,928.00 |    1,924,928.00 |            0.00 |
|[Counter] WithCache |      operations |        1,367.00 |        1,367.00 |        1,367.00 |            0.00 |

### Per-second Totals
|          Metric |       Units / s |         Max / s |     Average / s |         Min / s |      StdDev / s |
|---------------- |---------------- |---------------- |---------------- |---------------- |---------------- |
|TotalBytesAllocated |           bytes |   96,611,286.35 |   95,243,010.04 |   93,934,605.37 |    1,339,344.51 |
|[Counter] WithCache |      operations |       68,609.13 |       67,637.44 |       66,708.26 |          951.14 |

### Raw Data
#### TotalBytesAllocated
|           Run # |           bytes |       bytes / s |      ns / bytes |
|---------------- |---------------- |---------------- |---------------- |
|               1 |    1,924,928.00 |   93,934,605.37 |           10.65 |
|               2 |    1,924,928.00 |   95,183,138.41 |           10.51 |
|               3 |    1,924,928.00 |   96,611,286.35 |           10.35 |

#### [Counter] WithCache
|           Run # |      operations |  operations / s | ns / operations |
|---------------- |---------------- |---------------- |---------------- |
|               1 |        1,367.00 |       66,708.26 |       14,990.65 |
|               2 |        1,367.00 |       67,594.92 |       14,794.01 |
|               3 |        1,367.00 |       68,609.13 |       14,575.32 |


