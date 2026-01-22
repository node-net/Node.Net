using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Node.Net.Service.Logging;

/// <summary>
/// Logger provider that writes log entries to ILogService.
/// </summary>
/// <remarks>
/// This provider implements ILoggerProvider and creates logger instances that capture
/// log events from Microsoft.Extensions.Logging and store them in the ILogService.
/// </remarks>
public class LogServiceLoggerProvider : ILoggerProvider
{
    private readonly ILogService _logService;

    /// <summary>
    /// Initializes a new instance of the LogServiceLoggerProvider class.
    /// </summary>
    /// <param name="logService">The log service to write entries to.</param>
    /// <exception cref="ArgumentNullException">Thrown when logService is null.</exception>
    public LogServiceLoggerProvider(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
    }

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName)
    {
        return new LogServiceLogger(_logService, categoryName);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // No resources to dispose
    }

    /// <summary>
    /// Logger implementation that writes to ILogService.
    /// </summary>
    private sealed class LogServiceLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly ILogService _logService;
        private readonly string _categoryName;

        public LogServiceLogger(ILogService logService, string categoryName)
        {
            _logService = logService;
            _categoryName = categoryName;
        }

        IDisposable? Microsoft.Extensions.Logging.ILogger.BeginScope<TState>(TState state)
        {
            return null;
        }

        bool Microsoft.Extensions.Logging.ILogger.IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        void Microsoft.Extensions.Logging.ILogger.Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (formatter == null)
            {
                return;
            }

            try
            {
                var message = formatter(state, exception);
                var level = MapLogLevel((Microsoft.Extensions.Logging.LogLevel)logLevel);

                // Extract structured properties from state
                Dictionary<string, object>? properties = null;
                if (state is IReadOnlyList<KeyValuePair<string, object>> stateList)
                {
                    properties = new Dictionary<string, object>();
                    foreach (var kvp in stateList)
                    {
                        if (kvp.Key != "{OriginalFormat}")
                        {
                            properties[kvp.Key] = kvp.Value ?? string.Empty;
                        }
                    }
                }

                // Extract message template
                string? messageTemplate = null;
                if (state is IReadOnlyList<KeyValuePair<string, object>> stateList2)
                {
                    var originalFormat = stateList2.FirstOrDefault(kvp => kvp.Key == "{OriginalFormat}");
                    if (originalFormat.Value != null)
                    {
                        messageTemplate = originalFormat.Value.ToString();
                    }
                }

                // Serialize exception
                string? exceptionString = null;
                if (exception != null)
                {
                    exceptionString = exception.ToString();
                }

                var logEntry = new LogEntry
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    Level = level,
                    Message = message,
                    MessageTemplate = messageTemplate,
                    Properties = properties,
                    Exception = exceptionString,
                    SourceContext = _categoryName,
                    IsManualEntry = false
                };

                _logService.Create(logEntry);
            }
            catch
            {
                // Silently fail to avoid logging exceptions causing infinite loops
            }
        }

        private static string MapLogLevel(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return logLevel switch
            {
                Microsoft.Extensions.Logging.LogLevel.Trace => "Debug",
                Microsoft.Extensions.Logging.LogLevel.Debug => "Debug",
                Microsoft.Extensions.Logging.LogLevel.Information => "Info",
                Microsoft.Extensions.Logging.LogLevel.Warning => "Warn",
                Microsoft.Extensions.Logging.LogLevel.Error => "Error",
                Microsoft.Extensions.Logging.LogLevel.Critical => "Fatal",
                Microsoft.Extensions.Logging.LogLevel.None => "Info",
                _ => "Info"
            };
        }
    }
}
