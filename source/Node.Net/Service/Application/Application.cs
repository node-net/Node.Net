using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

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

    /// <summary>
    /// Gets the target framework moniker (TFM) for the application.
    /// </summary>
    /// <returns>The target framework moniker (e.g., "net8.0", "net8.0-windows", "net48"), or "unknown" if not available.</returns>
    public string GetTargetFramework()
    {
        try
        {
            var assembly = _assembly;
            
            // Method 1: Try to extract from assembly location path (most reliable)
            // Assemblies are typically in: bin/Debug/{TargetFramework}/ or bin/Release/{TargetFramework}/
            var assemblyLocation = assembly.Location;
            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                if (!string.IsNullOrEmpty(assemblyDir))
                {
                    var dirName = Path.GetFileName(assemblyDir);
                    // Check if directory name looks like a TFM (e.g., "net8.0", "net8.0-windows", "net48")
                    if (dirName != null && dirName.StartsWith("net", StringComparison.OrdinalIgnoreCase))
                    {
                        return dirName;
                    }
                    
                    // Check parent directory
                    var parentDir = Path.GetDirectoryName(assemblyDir);
                    if (!string.IsNullOrEmpty(parentDir))
                    {
                        var parentDirName = Path.GetFileName(parentDir);
                        if (parentDirName != null && parentDirName.StartsWith("net", StringComparison.OrdinalIgnoreCase))
                        {
                            return parentDirName;
                        }
                    }
                }
            }
            
            // Method 2: Use TargetFrameworkAttribute
            var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();
            if (targetFrameworkAttribute != null && !string.IsNullOrEmpty(targetFrameworkAttribute.FrameworkName))
            {
                var frameworkName = targetFrameworkAttribute.FrameworkName;
                
                if (frameworkName.Contains(".NETCoreApp"))
                {
                    // Extract version and determine TFM
                    var versionMatch = Regex.Match(frameworkName, @"Version=v(\d+)\.(\d+)");
                    if (versionMatch.Success)
                    {
                        var major = int.Parse(versionMatch.Groups[1].Value);
                        var minor = int.Parse(versionMatch.Groups[2].Value);
                        
                        // For .NET 8.0+, check if we have Windows-specific features
                        if (major >= 8)
                        {
                            // Check if we're running on net8.0-windows by looking for Windows-specific types
                            try
                            {
                                var windowsBaseType = Type.GetType("System.Windows.Application, WindowsBase, PresentationFramework");
                                if (windowsBaseType != null)
                                {
                                    return $"net{major}.{minor}-windows";
                                }
                            }
                            catch
                            {
                                // Ignore - not a Windows-specific framework
                            }
                            
                            return $"net{major}.{minor}";
                        }
                        
                        return $"net{major}.{minor}";
                    }
                }
                else if (frameworkName.Contains(".NETFramework"))
                {
                    // Extract version for .NET Framework
                    var versionMatch = Regex.Match(frameworkName, @"Version=v(\d+)\.(\d+)");
                    if (versionMatch.Success)
                    {
                        var major = int.Parse(versionMatch.Groups[1].Value);
                        var minor = int.Parse(versionMatch.Groups[2].Value);
                        return $"net{major}{minor}";
                    }
                }
            }
        }
        catch
        {
            // If we can't determine the framework, return "unknown"
        }
        
        return "unknown";
    }

    /// <summary>
    /// Gets the full filename (path) of the executing assembly.
    /// </summary>
    /// <returns>The full path to the executing assembly file, or empty string if not available.</returns>
    public string GetExecutingAssemblyFilename()
    {
        try
        {
            var assemblyLocation = _assembly.Location;
            return string.IsNullOrEmpty(assemblyLocation) ? string.Empty : assemblyLocation;
        }
        catch
        {
            // If we can't get the location, return empty string
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the version of the application.
    /// </summary>
    /// <returns>The version string from assembly metadata, or empty string if not available.</returns>
    public string GetVersion()
    {
        try
        {
            // First, try AssemblyInformationalVersionAttribute (most user-friendly, e.g., "1.2.3-beta")
            var informationalVersionAttribute = _assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (informationalVersionAttribute != null && !string.IsNullOrEmpty(informationalVersionAttribute.InformationalVersion))
            {
                return informationalVersionAttribute.InformationalVersion;
            }

            // Fall back to AssemblyVersion (e.g., "1.2.3.4")
            var version = _assembly.GetName().Version;
            if (version != null)
            {
                return version.ToString();
            }
        }
        catch
        {
            // If we can't get the version, return empty string
        }

        return string.Empty;
    }
}
