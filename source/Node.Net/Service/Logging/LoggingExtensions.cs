using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Node.Net.Service.Logging;

/// <summary>
/// Extension methods for registering Node.Net logging services.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Adds Node.Net logging to the service collection, configuring Microsoft.Extensions.Logging
    /// to route log messages to the specified ILogService.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="logService">The log service instance to use for storing log entries.</param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services or logService is null.</exception>
    /// <remarks>
    /// This method:
    /// 1. Registers the ILogService instance as a singleton (if not already registered)
    /// 2. Configures Microsoft.Extensions.Logging to use LogServiceLoggerProvider
    /// 3. Enables automatic capture of all application log messages
    /// </remarks>
    public static IServiceCollection AddNodeNetLogging(this IServiceCollection services, ILogService logService)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (logService == null)
        {
            throw new ArgumentNullException(nameof(logService));
        }

        // Register ILogService as singleton if not already registered
        services.AddSingleton(logService);

        // Configure Microsoft.Extensions.Logging to use our provider
        services.AddLogging(builder =>
        {
            builder.AddProvider(new LogServiceLoggerProvider(logService));
        });

        return services;
    }
}
