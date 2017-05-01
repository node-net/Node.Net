# Node.Net.Benchmark.FactoryVisual3DWithoutCache+WithoutCache
__Create Visual3D Without Caching__
_5/1/2017 3:55:45 PM_
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
|TotalBytesAllocated |           bytes |    6,683,328.00 |    5,968,381.33 |    5,028,448.00 |      850,071.29 |
|[Counter] WithoutCache |      operations |            2.00 |            2.00 |            2.00 |            0.00 |

### Per-second Totals
|          Metric |       Units / s |         Max / s |     Average / s |         Min / s |      StdDev / s |
|---------------- |---------------- |---------------- |---------------- |---------------- |---------------- |
|TotalBytesAllocated |           bytes |    7,421,881.21 |    6,632,190.44 |    5,667,996.58 |      889,868.71 |
|[Counter] WithoutCache |      operations |            2.25 |            2.22 |            2.20 |            0.03 |

### Raw Data
#### TotalBytesAllocated
|           Run # |           bytes |       bytes / s |      ns / bytes |
|---------------- |---------------- |---------------- |---------------- |
|               1 |    6,683,328.00 |    7,421,881.21 |          134.74 |
|               2 |    5,028,448.00 |    5,667,996.58 |          176.43 |
|               3 |    6,193,368.00 |    6,806,693.52 |          146.91 |

#### [Counter] WithoutCache
|           Run # |      operations |  operations / s | ns / operations |
|---------------- |---------------- |---------------- |---------------- |
|               1 |            2.00 |            2.22 |  450,244,878.01 |
|               2 |            2.00 |            2.25 |  443,582,483.70 |
|               3 |            2.00 |            2.20 |  454,946,882.89 |


