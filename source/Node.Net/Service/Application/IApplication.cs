namespace Node.Net.Service.Application;

/// <summary>
/// Interface for accessing application metadata
/// </summary>
public interface IApplication
{
    /// <summary>
    /// Gets all application information in a single call.
    /// </summary>
    /// <returns>An <see cref="ApplicationInfo"/> instance containing all application metadata.</returns>
    ApplicationInfo GetApplicationInfo();
}
