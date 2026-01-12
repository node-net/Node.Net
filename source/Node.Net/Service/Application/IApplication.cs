using System.IO;

namespace Node.Net.Service.Application;

/// <summary>
/// Interface for accessing application metadata
/// </summary>
public interface IApplication
{
    /// <summary>
    /// Gets the application name
    /// </summary>
    /// <returns>The application name derived from assembly metadata, or assembly name if metadata not available, or "Unknown" if assembly name is null</returns>
    string GetName();
    
    /// <summary>
    /// Gets the company name
    /// </summary>
    /// <returns>The company name from assembly metadata, or empty string if not available</returns>
    string GetCompany();
    
    /// <summary>
    /// Gets the application data directory
    /// </summary>
    /// <returns>A DirectoryInfo object pointing to the application's data directory. The directory is created if it does not exist.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory cannot be created due to invalid path</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to create the directory is denied</exception>
    DirectoryInfo GetApplicationDataDirectory();
}
