# Node.Net.Benchmark.FactoryVisual3DWithCache+WithCache
__Create Visual3D With Caching__
_5/1/2017 3:55:26 PM_
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
|TotalBytesAllocated |           bytes |    3,810,992.00 |    3,810,992.00 |    3,810,992.00 |            0.00 |
|[Counter] WithCache |      operations |        1,590.00 |        1,590.00 |        1,590.00 |            0.00 |

### Per-second Totals
|          Metric |       Units / s |         Max / s |     Average / s |         Min / s |      StdDev / s |
|---------------- |---------------- |---------------- |---------------- |---------------- |---------------- |
|TotalBytesAllocated |           bytes |  158,019,863.69 |  156,446,610.16 |  154,806,407.88 |    1,607,773.67 |
|[Counter] WithCache |      operations |       65,928.13 |       65,271.75 |       64,587.43 |          670.79 |

### Raw Data
#### TotalBytesAllocated
|           Run # |           bytes |       bytes / s |      ns / bytes |
|---------------- |---------------- |---------------- |---------------- |
|               1 |    3,810,992.00 |  158,019,863.69 |            6.33 |
|               2 |    3,810,992.00 |  154,806,407.88 |            6.46 |
|               3 |    3,810,992.00 |  156,513,558.91 |            6.39 |

#### [Counter] WithCache
|           Run # |      operations |  operations / s | ns / operations |
|---------------- |---------------- |---------------- |---------------- |
|               1 |        1,590.00 |       65,928.13 |       15,168.03 |
|               2 |        1,590.00 |       64,587.43 |       15,482.89 |
|               3 |        1,590.00 |       65,299.68 |       15,314.01 |


