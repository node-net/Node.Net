# Node.Net
[![NuGet Version and Downloads count](https://buildstats.info/nuget/Node.Net)](https://www.nuget.org/packages/Node.Net)
![license](https://img.shields.io/github/license/node-net/Node.Net.svg)
Node.Net is a framework library to support IDictionary data operations, including serialization to and from JSON

## Table of Contents ##

- [Downloads](#downloads)
- [Cross-Platform Compatibility](#cross-platform-compatibility)
- [License](#license)

## Downloads ##

The latest release of Node.Net is [available on NuGet](https://www.nuget.org/packages/Node.Net/).

## Cross-Platform Compatibility ##

Node.Net supports multiple target frameworks:
- **Windows:** `net48`, `net8.0-windows` (uses framework-provided WPF types)
- **Cross-Platform:** `net8.0`, `net8.0-wasm` (provides `System.Windows.*` type implementations)

For detailed information about how `System.Windows.*` types are conditionally compiled and used across different target frameworks, see [docs/SYSTEM_NAMESPACE_RULES.md](docs/SYSTEM_NAMESPACE_RULES.md).

## License ##

Node.Net is Open Source software and is released under the [MIT license](https://github.com/node-net/Node.Net/wiki/License).