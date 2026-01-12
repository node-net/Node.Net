using System;
using System.IO;
using System.Reflection;

namespace Node.Net.Service.Application;

/// <summary>
/// Concrete implementation of IApplication that uses reflection to read executing assembly metadata
/// </summary>
public class Application : IApplication
{
    private readonly Assembly _assembly;
    private string? _cachedName;
    private string? _cachedCompany;

    /// <summary>
    /// Initializes a new instance of the Application class
    /// </summary>
    public Application()
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    /// <summary>
    /// Gets the application name derived from the executing assembly's metadata
    /// </summary>
    /// <returns>The application name using this precedence: (1) AssemblyTitleAttribute.Title if present, (2) Assembly.GetName().Name if present, (3) "Unknown" if assembly name is null. Never returns null.</returns>
    public string GetName()
    {
        if (_cachedName != null)
        {
            return _cachedName;
        }

        var titleAttribute = _assembly.GetCustomAttribute<AssemblyTitleAttribute>();
        if (titleAttribute != null && !string.IsNullOrEmpty(titleAttribute.Title))
        {
            _cachedName = titleAttribute.Title;
            return _cachedName;
        }

        var assemblyName = _assembly.GetName().Name;
        _cachedName = assemblyName ?? "Unknown";
        return _cachedName;
    }

    /// <summary>
    /// Gets the company name from the executing assembly's metadata
    /// </summary>
    /// <returns>The company name from AssemblyCompany attribute if present, otherwise empty string. Empty string is a valid return value indicating no company metadata.</returns>
    public string GetCompany()
    {
        if (_cachedCompany != null)
        {
            return _cachedCompany;
        }

        var companyAttribute = _assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
        _cachedCompany = companyAttribute?.Company ?? string.Empty;
        return _cachedCompany;
    }

    /// <summary>
    /// Gets the application data directory
    /// </summary>
    /// <returns>A DirectoryInfo object pointing to the application's data directory. The directory is created if it does not exist.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory cannot be created due to invalid path</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to create the directory is denied</exception>
    public DirectoryInfo GetApplicationDataDirectory()
    {
        var appDataBase = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var company = GetCompany();
        var appName = GetName();
        
        var appDataPath = Path.Combine(appDataBase, company, appName);
        
        try
        {
            Directory.CreateDirectory(appDataPath);
            return new DirectoryInfo(appDataPath);
        }
        catch (DirectoryNotFoundException ex)
        {
            throw new DirectoryNotFoundException($"Cannot create application data directory at '{appDataPath}': Invalid path.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"Cannot create application data directory at '{appDataPath}': Access denied.", ex);
        }
    }
}
