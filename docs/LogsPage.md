# Logs Page

implemented in both examples/Node.Net.AspNet.Host and examples/Node.Net.Wasm

## Model

Node.Net/Service/Logging/LogEntry a C# class compatible with Serilog fully structure logging
Node.Net/Service/Logging/ILogService an interface for CRUD operations on LogEntry, as well as a search method
Node.Net/Service/Logging/LogService a concrete implementation of ILogService, backed by a LiteDB database APPLICATION_DATA_DIRECTORY/log.db 

provides a configurationm
ethod to configure Microsoft.Extension.Logging to route message to ILogService

## User interface

Node.Net.Components.Logs a component that leverage ILogService to display log entries
