using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
#if !IS_FRAMEWORK && IS_WINDOWS
using System.Threading.Tasks;
#endif

namespace Node.Net.Service.User;

/// <summary>
/// Service for retrieving the OS user profile picture
/// </summary>
public class OsUserProfileService
{
    /// <summary>
    /// Gets the OS user profile picture as a base64-encoded data URL
    /// </summary>
    public string? GetUserProfilePicture()
    {
        var result = GetUserProfilePictureWithDiagnostics();
        return result.ProfilePictureUrl;
    }

    /// <summary>
    /// Gets the OS user profile picture with diagnostic information
    /// </summary>
    public OsUserProfileResult GetUserProfilePictureWithDiagnostics()
    {
        var result = new OsUserProfileResult();
        var diagnosticInfo = new System.Text.StringBuilder();
        
        try
        {
            var userName = Environment.UserName;
            var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            
            diagnosticInfo.AppendLine($"User Name: {userName}");
            diagnosticInfo.AppendLine($"Home Directory: {homeDir}");
            diagnosticInfo.AppendLine($"OS Platform: {RuntimeInformation.OSDescription}");
            diagnosticInfo.AppendLine();
            
            var picturePath = GetProfilePicturePath(result, diagnosticInfo);
            
            if (string.IsNullOrEmpty(picturePath) || !File.Exists(picturePath))
            {
                result.ErrorMessage = "Profile picture path not found or file does not exist";
                result.DiagnosticInfo = diagnosticInfo.ToString();
                return result;
            }

            diagnosticInfo.AppendLine($"✅ Found profile picture at: {picturePath}");
            diagnosticInfo.AppendLine($"File size: {new FileInfo(picturePath).Length} bytes");
            diagnosticInfo.AppendLine($"File exists: {File.Exists(picturePath)}");
            
            var imageBytes = File.ReadAllBytes(picturePath);
            var extension = Path.GetExtension(picturePath).ToLowerInvariant();
            
            // Convert HEIC/HEIF to JPEG using macOS sips command (browsers don't support HEIC)
            byte[] finalImageBytes;
            string contentType;
            
            if (extension == ".heic" || extension == ".heif")
            {
                // Use sips to convert HEIC to JPEG
                var tempJpegPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");
                
                try
                {
                    var sipsStartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "/usr/bin/sips",
                        Arguments = $"-s format jpeg \"{picturePath}\" --out \"{tempJpegPath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    
                    using var sipsProcess = System.Diagnostics.Process.Start(sipsStartInfo);
                    if (sipsProcess != null)
                    {
                        sipsProcess.WaitForExit();
                        if (sipsProcess.ExitCode == 0 && File.Exists(tempJpegPath))
                        {
                            finalImageBytes = File.ReadAllBytes(tempJpegPath);
                            contentType = "image/jpeg";
                            
                            // Clean up temp file
                            try { File.Delete(tempJpegPath); } catch { }
                            diagnosticInfo.AppendLine($"✅ Converted HEIC to JPEG");
                        }
                        else
                        {
                            // Conversion failed, try reading original (will likely fail in browser)
                            finalImageBytes = imageBytes;
                            contentType = "image/heic";
                            diagnosticInfo.AppendLine($"⚠️ HEIC conversion failed, using original (may not display in browser)");
                        }
                    }
                    else
                    {
                        finalImageBytes = imageBytes;
                        contentType = "image/heic";
                    }
                }
                catch (Exception ex)
                {
                    diagnosticInfo.AppendLine($"⚠️ HEIC conversion exception: {ex.Message}");
                    finalImageBytes = imageBytes;
                    contentType = "image/heic";
                }
            }
            else
            {
                finalImageBytes = imageBytes;
                contentType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".bmp" => "image/bmp",
                    ".webp" => "image/webp",
                    _ => "image/jpeg" // Default to JPEG
                };
            }
            
            var base64 = Convert.ToBase64String(finalImageBytes);
            result.ProfilePictureUrl = $"data:{contentType};base64,{base64}";
            result.DiagnosticInfo = diagnosticInfo.ToString();
            return result;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = $"Error reading profile picture: {ex.Message}";
            diagnosticInfo.AppendLine($"❌ Exception: {ex.GetType().Name}");
            diagnosticInfo.AppendLine($"Message: {ex.Message}");
            diagnosticInfo.AppendLine($"Stack Trace: {ex.StackTrace}");
            result.DiagnosticInfo = diagnosticInfo.ToString();
            return result;
        }
    }

    /// <summary>
    /// Gets the path to the user's profile picture based on the OS
    /// </summary>
    private string? GetProfilePicturePath(OsUserProfileResult result, System.Text.StringBuilder diagnosticInfo)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return GetWindowsProfilePicturePath(result, diagnosticInfo);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return GetMacOSProfilePicturePath(result, diagnosticInfo);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return GetLinuxProfilePicturePath(result, diagnosticInfo);
        }

        diagnosticInfo.AppendLine("❌ Unknown OS platform");
        return null;
    }

    /// <summary>
    /// Gets the Windows user profile picture path
    /// Uses WinRT API (Windows.System.UserProfile.UserInformation) for .NET 8.0+, falls back to file system for .NET Framework 4.8
    /// </summary>
    private string? GetWindowsProfilePicturePath(OsUserProfileResult result, System.Text.StringBuilder diagnosticInfo)
    {
        try
        {
            diagnosticInfo.AppendLine("=== Windows Profile Picture Search ===");
            
#if !IS_FRAMEWORK
            // Try WinRT API first (available on .NET 8.0+)
            diagnosticInfo.AppendLine("Method 1: Attempting WinRT API (Windows.System.UserProfile.UserInformation)...");
            try
            {
                var winrtPath = TryGetWindowsProfilePictureViaWinRT(result, diagnosticInfo);
                if (!string.IsNullOrEmpty(winrtPath) && File.Exists(winrtPath))
                {
                    diagnosticInfo.AppendLine($"✅ Successfully retrieved profile picture via WinRT API: {winrtPath}");
                    return winrtPath;
                }
            }
            catch (Exception ex)
            {
                diagnosticInfo.AppendLine($"   ⚠️ WinRT API failed: {ex.Message}");
                diagnosticInfo.AppendLine($"   Type: {ex.GetType().Name}");
            }
            
            diagnosticInfo.AppendLine();
            diagnosticInfo.AppendLine("Method 2: Falling back to file system search...");
#endif
            
            // Fallback to file system approach (works for .NET Framework 4.8 and as fallback for .NET 8.0+)
            var userName = Environment.UserName;
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            
            // Try multiple common locations for Windows profile pictures
            var possiblePaths = new[]
            {
                // Windows 10/11: AccountPictures folder
                Path.Combine(userProfile, "AppData", "Roaming", "Microsoft", "Windows", "AccountPictures", $"{userName}_AccountPicture.jpg"),
                Path.Combine(userProfile, "AppData", "Roaming", "Microsoft", "Windows", "AccountPictures", $"{userName}_AccountPicture.png"),
                
                // Alternative location
                Path.Combine(userProfile, "AppData", "Roaming", "Microsoft", "Windows", "AccountPictures", $"{userName}_AccountPicture.dat"),
                
                // Check for any file in AccountPictures folder
                Path.Combine(userProfile, "AppData", "Roaming", "Microsoft", "Windows", "AccountPictures"),
            };

            foreach (var path in possiblePaths)
            {
                result.AttemptedPaths.Add(path);
                
                if (File.Exists(path))
                {
                    diagnosticInfo.AppendLine($"✅ Found picture file: {path}");
                    result.FoundPaths.Add(path);
                    return path;
                }
                
                // If it's a directory, look for image files inside
                if (Directory.Exists(path))
                {
                    diagnosticInfo.AppendLine($"📁 Checking directory: {path}");
                    var imageFiles = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(f => IsImageFile(f))
                        .OrderByDescending(f => File.GetLastWriteTime(f))
                        .FirstOrDefault();
                    
                    if (imageFiles != null)
                    {
                        diagnosticInfo.AppendLine($"✅ Found image file in directory: {imageFiles}");
                        result.FoundPaths.Add(imageFiles);
                        return imageFiles;
                    }
                    diagnosticInfo.AppendLine($"   No image files found in directory");
                }
                else
                {
                    diagnosticInfo.AppendLine($"❌ Path does not exist: {path}");
                }
            }

            diagnosticInfo.AppendLine("❌ No profile picture found in any checked location");
            return null;
        }
        catch (Exception ex)
        {
            diagnosticInfo.AppendLine($"❌ Exception in GetWindowsProfilePicturePath: {ex.Message}");
            return null;
        }
    }

#if !IS_FRAMEWORK
    /// <summary>
    /// Attempts to retrieve Windows profile picture using WinRT API (Windows.System.UserProfile.UserInformation)
    /// This is the recommended approach for Windows 10/11 as it works for both local and Microsoft accounts.
    /// Uses reflection to avoid requiring explicit WinRT package references.
    /// </summary>
    private string? TryGetWindowsProfilePictureViaWinRT(OsUserProfileResult result, System.Text.StringBuilder diagnosticInfo)
    {
        try
        {
            // Use reflection to access WinRT APIs (works without explicit package references)
            // Try multiple type name formats for better compatibility
            var userInformationType = Type.GetType("Windows.System.UserProfile.UserInformation, Windows, ContentType=WindowsRuntime") 
                ?? Type.GetType("Windows.System.UserProfile.UserInformation, Windows.System.UserProfile")
                ?? Type.GetType("Windows.System.UserProfile.UserInformation");
            
            if (userInformationType == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ WinRT UserInformation type not available (Windows Runtime not accessible)");
                diagnosticInfo.AppendLine("   Note: WinRT APIs require Windows 10/11 and proper runtime support");
                diagnosticInfo.AppendLine("   Attempted type names:");
                diagnosticInfo.AppendLine("     - Windows.System.UserProfile.UserInformation, Windows, ContentType=WindowsRuntime");
                diagnosticInfo.AppendLine("     - Windows.System.UserProfile.UserInformation, Windows.System.UserProfile");
                diagnosticInfo.AppendLine("     - Windows.System.UserProfile.UserInformation");
                return null;
            }

            // Check if account picture is available
            var isAvailableProperty = userInformationType.GetProperty("IsAccountPictureAvailable");
            if (isAvailableProperty == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ IsAccountPictureAvailable property not found");
                return null;
            }

            var isAvailable = (bool)(isAvailableProperty.GetValue(null) ?? false);
            if (!isAvailable)
            {
                diagnosticInfo.AppendLine("   ⚠️ Account picture is not available (IsAccountPictureAvailable = false)");
                return null;
            }

            diagnosticInfo.AppendLine("   ✅ Account picture is available");

            // Get AccountPictureKind enum
            var accountPictureKindType = Type.GetType("Windows.System.UserProfile.AccountPictureKind, Windows, ContentType=WindowsRuntime");
            if (accountPictureKindType == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ AccountPictureKind type not found");
                return null;
            }

            var largeImageValue = Enum.Parse(accountPictureKindType, "LargeImage");

            // Call GetAccountPicture (async method)
            var getAccountPictureMethod = userInformationType.GetMethod("GetAccountPicture", new[] { accountPictureKindType });
            if (getAccountPictureMethod == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ GetAccountPicture method not found");
                return null;
            }

            var task = getAccountPictureMethod.Invoke(null, new[] { largeImageValue });
            if (task == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ GetAccountPicture returned null");
                return null;
            }

            // Get result from Task<StorageFile>
            var taskType = task.GetType();
            var getAwaiterMethod = taskType.GetMethod("GetAwaiter");
            if (getAwaiterMethod == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ Task.GetAwaiter method not found");
                return null;
            }

            var awaiter = getAwaiterMethod.Invoke(task, null);
            if (awaiter == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ GetAwaiter returned null");
                return null;
            }

            var getResultMethod = awaiter.GetType().GetMethod("GetResult");
            if (getResultMethod == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ GetResult method not found");
                return null;
            }

            var storageFile = getResultMethod.Invoke(awaiter, null);
            if (storageFile == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ GetAccountPicture returned null StorageFile");
                return null;
            }

            var storageFileType = storageFile.GetType();

            // Try to get file path first
            var pathProperty = storageFileType.GetProperty("Path");
            if (pathProperty != null)
            {
                var filePath = pathProperty.GetValue(storageFile) as string;
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    diagnosticInfo.AppendLine($"   ✅ Retrieved profile picture via WinRT: {filePath}");
                    result.FoundPaths.Add(filePath);
                    return filePath;
                }
            }

            // Read file content if path not available
            diagnosticInfo.AppendLine("   Reading file content from WinRT StorageFile...");
            var openReadAsyncMethod = storageFileType.GetMethod("OpenReadAsync");
            if (openReadAsyncMethod == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ OpenReadAsync method not found");
                return null;
            }

            var readTask = openReadAsyncMethod.Invoke(storageFile, null);
            if (readTask == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ OpenReadAsync returned null");
                return null;
            }

            var readTaskType = readTask.GetType();
            var readAwaiter = readTaskType.GetMethod("GetAwaiter")?.Invoke(readTask, null);
            if (readAwaiter == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ Read task GetAwaiter returned null");
                return null;
            }

            var readResult = readAwaiter.GetType().GetMethod("GetResult")?.Invoke(readAwaiter, null);
            if (readResult == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ Read stream is null");
                return null;
            }

            // Convert IRandomAccessStream to .NET Stream
            var streamExtensionsType = Type.GetType("System.IO.WindowsRuntimeStreamExtensions, System.Runtime.WindowsRuntime");
            if (streamExtensionsType == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ WindowsRuntimeStreamExtensions type not found");
                return null;
            }

            var asStreamForReadMethod = streamExtensionsType.GetMethod("AsStreamForRead", new[] { readResult.GetType() });
            if (asStreamForReadMethod == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ AsStreamForRead method not found");
                return null;
            }

            var netStream = asStreamForReadMethod.Invoke(null, new[] { readResult }) as Stream;
            if (netStream == null)
            {
                diagnosticInfo.AppendLine("   ⚠️ Failed to convert to .NET Stream");
                return null;
            }

            using (netStream)
            {
                using var ms = new MemoryStream();
                netStream.CopyTo(ms);
                var imageBytes = ms.ToArray();

                // Write to temp file
                var tempFile = Path.Combine(Path.GetTempPath(), $"user_picture_{Environment.UserName}_{Guid.NewGuid()}.jpg");
                File.WriteAllBytes(tempFile, imageBytes);
                
                diagnosticInfo.AppendLine($"   ✅ Retrieved profile picture via WinRT (saved to temp): {tempFile} ({imageBytes.Length} bytes)");
                result.FoundPaths.Add(tempFile);
                return tempFile;
            }
        }
        catch (Exception ex)
        {
            diagnosticInfo.AppendLine($"   ❌ WinRT API exception: {ex.GetType().Name}: {ex.Message}");
            diagnosticInfo.AppendLine($"   Stack trace: {ex.StackTrace}");
            return null;
        }
    }
#endif

    /// <summary>
    /// Gets the macOS user profile picture path
    /// Uses the canonical Directory Services approach: JPEGPhoto (raw bytes) first, then Picture (file path)
    /// </summary>
    private string? GetMacOSProfilePicturePath(OsUserProfileResult result, System.Text.StringBuilder diagnosticInfo)
    {
        try
        {
            var userName = Environment.UserName;
            var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            
            diagnosticInfo.AppendLine("=== macOS Profile Picture Search ===");
            diagnosticInfo.AppendLine("Using canonical Directory Services approach:");
            diagnosticInfo.AppendLine("  1. JPEGPhoto (raw bytes from directory record) - preferred");
            diagnosticInfo.AppendLine("  2. Picture (file path) - fallback");
            diagnosticInfo.AppendLine("  3. NSImage API (for virtualized pictures) - last resort");
            diagnosticInfo.AppendLine();
            
            // Method 1: Try JPEGPhoto first (canonical source, raw bytes in directory record)
            diagnosticInfo.AppendLine("Method 1: Reading JPEGPhoto from Directory Services...");
            diagnosticInfo.AppendLine("   This is the preferred method - many corporate/managed Macs store photos here");
            try
            {
                var jpegPhotoBytes = TryReadDsclJpegPhoto(userName, result, diagnosticInfo);
                if (jpegPhotoBytes != null && jpegPhotoBytes.Length > 0)
                {
                    // Write to temp file and return path
                    var tempFile = Path.Combine(Path.GetTempPath(), $"user_picture_{userName}_{Guid.NewGuid()}.jpg");
                    File.WriteAllBytes(tempFile, jpegPhotoBytes);
                    
                    var fileInfo = new FileInfo(tempFile);
                    diagnosticInfo.AppendLine($"✅ Successfully retrieved JPEGPhoto: {tempFile} ({fileInfo.Length} bytes)");
                    result.FoundPaths.Add(tempFile);
                    return tempFile;
                }
            }
            catch (Exception ex)
            {
                diagnosticInfo.AppendLine($"   ❌ JPEGPhoto read failed: {ex.Message}");
            }

            // Method 2: Fall back to Picture attribute (file path)
            diagnosticInfo.AppendLine();
            diagnosticInfo.AppendLine("Method 2: Reading Picture path from Directory Services...");
            string? dsclPicturePath = null;
            try
            {
                dsclPicturePath = TryReadDsclPicturePath(userName, result, diagnosticInfo);
                if (!string.IsNullOrEmpty(dsclPicturePath) && File.Exists(dsclPicturePath) && dsclPicturePath != null && IsImageFile(dsclPicturePath))
                {
                    diagnosticInfo.AppendLine($"✅ Using picture from dscl Picture attribute: {dsclPicturePath}");
                    return dsclPicturePath;
                }
                else if (!string.IsNullOrEmpty(dsclPicturePath))
                {
                    diagnosticInfo.AppendLine($"⚠️ Picture path from dscl doesn't exist or isn't an image: {dsclPicturePath}");
                    diagnosticInfo.AppendLine($"   File exists: {File.Exists(dsclPicturePath)}");
                    if (dsclPicturePath != null && File.Exists(dsclPicturePath))
                    {
                        diagnosticInfo.AppendLine($"   Is image: {IsImageFile(dsclPicturePath)}");
                    }
                    diagnosticInfo.AppendLine($"   Note: Picture attribute often points to system defaults (e.g., Eagle)");
                }
            }
            catch (Exception ex)
            {
                diagnosticInfo.AppendLine($"   ❌ dscl Picture read failed: {ex.Message}");
            }

            // Method 3: Use NSImage API via AppleScript (for virtualized pictures on modern macOS)
            diagnosticInfo.AppendLine();
            diagnosticInfo.AppendLine("Method 3: Using NSImage API via osascript (for virtualized pictures)...");
            diagnosticInfo.AppendLine("   This uses the native macOS API that works for virtualized pictures");
            try
            {
                var tempFile = Path.Combine(Path.GetTempPath(), $"user_picture_{userName}_{Guid.NewGuid()}.png");
                result.AttemptedPaths.Add($"NSImage export to: {tempFile}");
                
                // Use AppleScript with NSImage API
                var appleScript = $@"use framework ""AppKit""
set img to (current application's NSImage's imageNamed:""NSUser"")
if img is not missing value then
    set tiffData to img's TIFFRepresentation()
    set tempTiffFile to ""{tempFile.Replace(".png", ".tiff")}""
    tiffData's writeToFile:tempTiffFile atomically:true
    return tempTiffFile
else
    return """"
end if";
                
                var osascriptProcess = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "/usr/bin/osascript",
                    Arguments = $"-e \"{appleScript.Replace("\"", "\\\"")}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var osascriptProc = System.Diagnostics.Process.Start(osascriptProcess);
                if (osascriptProc != null)
                {
                    var output = osascriptProc.StandardOutput.ReadToEnd();
                    var error = osascriptProc.StandardError.ReadToEnd();
                    osascriptProc.WaitForExit();
                    
                    diagnosticInfo.AppendLine($"   osascript exit code: {osascriptProc.ExitCode}");
                    if (!string.IsNullOrEmpty(output))
                    {
                        diagnosticInfo.AppendLine($"   osascript output: {output.Trim()}");
                    }
                    if (!string.IsNullOrEmpty(error))
                    {
                        diagnosticInfo.AppendLine($"   osascript error: {error}");
                    }
                    
                    // Use the actual path returned by AppleScript
                    var tempTiffFile = output?.Trim();
                    if (string.IsNullOrEmpty(tempTiffFile))
                    {
                        tempTiffFile = tempFile.Replace(".png", ".tiff");
                    }
                    
                    diagnosticInfo.AppendLine($"   Checking for TIFF file at: {tempTiffFile}");
                    if (!string.IsNullOrEmpty(tempTiffFile) && File.Exists(tempTiffFile) && tempTiffFile != null && IsImageFile(tempTiffFile))
                    {
                        // Convert TIFF to PNG using sips
                        try
                        {
                            var sipsStartInfo = new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = "/usr/bin/sips",
                                Arguments = $"-s format png \"{tempTiffFile}\" --out \"{tempFile}\"",
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };
                            
                            using var sipsProcess = System.Diagnostics.Process.Start(sipsStartInfo);
                            if (sipsProcess != null)
                            {
                                sipsProcess.WaitForExit();
                                if (sipsProcess.ExitCode == 0 && File.Exists(tempFile))
                                {
                                    // Clean up TIFF file
                                    try { File.Delete(tempTiffFile); } catch { }
                                    
                                    var fileInfo = new FileInfo(tempFile);
                                    diagnosticInfo.AppendLine($"✅ Successfully exported picture using NSImage API: {tempFile} ({fileInfo.Length} bytes)");
                                    result.FoundPaths.Add(tempFile);
                                    return tempFile;
                                }
                            }
                        }
                        catch (Exception sipsEx)
                        {
                            diagnosticInfo.AppendLine($"   ⚠️ Failed to convert TIFF to PNG: {sipsEx.Message}");
                        }
                    }
                    
                    diagnosticInfo.AppendLine($"   ❌ NSImage API did not create a valid image file");
                }
            }
            catch (Exception ex)
            {
                diagnosticInfo.AppendLine($"   ❌ NSImage API approach failed: {ex.Message}");
            }

            // Method 3: Check UserPictureSyncAgent directory (where macOS stores synced user pictures)
            diagnosticInfo.AppendLine();
            diagnosticInfo.AppendLine("Method 3: Checking UserPictureSyncAgent directory...");
            
            var possiblePaths = new[]
            {
                // UserPictureSyncAgent container (where macOS stores synced user pictures)
                Path.Combine(homeDir, "Library", "Containers", "com.apple.UserPictureSyncAgent", "Data", "Library", "User Pictures", "Pictures"),
                Path.Combine(homeDir, "Library", "Containers", "com.apple.UserPictureSyncAgent", "Data", "Library", "User Pictures"),
                
                // System library user pictures
                $"/System/Library/User Pictures/{userName}.tif",
                $"/System/Library/User Pictures/{userName}.png",
                $"/System/Library/User Pictures/{userName}.jpg",
                
                // Library user pictures
                $"/Library/User Pictures/{userName}.tif",
                $"/Library/User Pictures/{userName}.png",
                $"/Library/User Pictures/{userName}.jpg",
                
                // Check in User Pictures directories
                "/System/Library/User Pictures",
                "/Library/User Pictures",
                
                // User's Pictures folder
                Path.Combine(homeDir, "Pictures", "Photo Booth Library", "Pictures"),
                Path.Combine(homeDir, "Pictures"),
                
                // Check for .face file (Linux-style, sometimes used)
                Path.Combine(homeDir, ".face"),
            };

            foreach (var path in possiblePaths)
            {
                result.AttemptedPaths.Add(path);
                
                if (File.Exists(path) && IsImageFile(path))
                {
                    diagnosticInfo.AppendLine($"✅ Found picture file: {path}");
                    result.FoundPaths.Add(path);
                    return path;
                }
                
                if (Directory.Exists(path))
                {
                    diagnosticInfo.AppendLine($"📁 Checking directory: {path}");
                    
                    // Special handling for UserPictureSyncAgent Pictures directory
                    if (path.Contains("UserPictureSyncAgent") && path.EndsWith("Pictures"))
                    {
                        diagnosticInfo.AppendLine("   This is the UserPictureSyncAgent Pictures directory - looking for most recent UUID-named image");
                        var imageFiles = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                            .Where(f => IsImageFile(f))
                            .OrderByDescending(f => File.GetLastWriteTime(f))
                            .ToList();
                        
                        if (imageFiles.Any())
                        {
                            diagnosticInfo.AppendLine($"   Found {imageFiles.Count} image file(s)");
                            foreach (var img in imageFiles)
                            {
                                var fileInfo = new FileInfo(img);
                                diagnosticInfo.AppendLine($"     - {Path.GetFileName(img)} ({fileInfo.Length} bytes, modified: {fileInfo.LastWriteTime})");
                                result.FoundPaths.Add(img);
                            }
                            
                            var mostRecent = imageFiles.First();
                            diagnosticInfo.AppendLine($"✅ Using most recently modified: {mostRecent}");
                            return mostRecent;
                        }
                        else
                        {
                            diagnosticInfo.AppendLine($"   No image files found in UserPictureSyncAgent Pictures directory");
                        }
                    }
                    else
                    {
                        // Look for files matching the username
                        var userNameFiles = Directory.GetFiles(path, $"{userName}.*", SearchOption.TopDirectoryOnly)
                            .Where(f => IsImageFile(f))
                            .FirstOrDefault();
                        
                        if (userNameFiles != null)
                        {
                            diagnosticInfo.AppendLine($"✅ Found username-matching file: {userNameFiles}");
                            result.FoundPaths.Add(userNameFiles);
                            return userNameFiles;
                        }
                        
                        // Look for any image files (most recent first)
                        var imageFiles = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                            .Where(f => IsImageFile(f))
                            .OrderByDescending(f => File.GetLastWriteTime(f))
                            .ToList();
                        
                        if (imageFiles.Any())
                        {
                            diagnosticInfo.AppendLine($"   Found {imageFiles.Count} image file(s)");
                            foreach (var img in imageFiles.Take(5)) // Show first 5
                            {
                                var fileInfo = new FileInfo(img);
                                diagnosticInfo.AppendLine($"     - {Path.GetFileName(img)} ({fileInfo.Length} bytes)");
                                result.FoundPaths.Add(img);
                            }
                            
                            var mostRecent = imageFiles.First();
                            diagnosticInfo.AppendLine($"✅ Using most recently modified: {mostRecent}");
                            return mostRecent;
                        }
                        
                        diagnosticInfo.AppendLine($"   No image files found in directory");
                    }
                }
                else
                {
                    diagnosticInfo.AppendLine($"❌ Path does not exist: {path}");
                }
            }

            diagnosticInfo.AppendLine();
            diagnosticInfo.AppendLine("❌ No profile picture found in any checked location");
            return null;
        }
        catch (Exception ex)
        {
            diagnosticInfo.AppendLine($"❌ Exception in GetMacOSProfilePicturePath: {ex.Message}");
            diagnosticInfo.AppendLine($"Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    /// <summary>
    /// Gets the Linux user profile picture path
    /// </summary>
    private string? GetLinuxProfilePicturePath(OsUserProfileResult result, System.Text.StringBuilder diagnosticInfo)
    {
        try
        {
            var userName = Environment.UserName;
            var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            
            diagnosticInfo.AppendLine("=== Linux Profile Picture Search ===");
            
            // Common Linux locations for user profile pictures
            var possiblePaths = new[]
            {
                // Standard .face file
                Path.Combine(homeDir, ".face"),
                
                // Alternative locations
                Path.Combine(homeDir, ".face.icon"),
                Path.Combine(homeDir, ".avatar"),
                
                // System-wide location
                $"/var/lib/AccountsService/icons/{userName}",
                
                // User Pictures folder
                Path.Combine(homeDir, "Pictures"),
            };

            foreach (var path in possiblePaths)
            {
                result.AttemptedPaths.Add(path);
                
                if (File.Exists(path) && IsImageFile(path))
                {
                    diagnosticInfo.AppendLine($"✅ Found picture file: {path}");
                    result.FoundPaths.Add(path);
                    return path;
                }
                
                if (Directory.Exists(path))
                {
                    diagnosticInfo.AppendLine($"📁 Checking directory: {path}");
                    var imageFiles = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(f => IsImageFile(f))
                        .OrderByDescending(f => File.GetLastWriteTime(f))
                        .FirstOrDefault();
                    
                    if (imageFiles != null)
                    {
                        diagnosticInfo.AppendLine($"✅ Found image file in directory: {imageFiles}");
                        result.FoundPaths.Add(imageFiles);
                        return imageFiles;
                    }
                    diagnosticInfo.AppendLine($"   No image files found in directory");
                }
                else
                {
                    diagnosticInfo.AppendLine($"❌ Path does not exist: {path}");
                }
            }

            diagnosticInfo.AppendLine("❌ No profile picture found in any checked location");
            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Checks if a file is an image file based on extension
    /// </summary>
    private bool IsImageFile(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension is ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" or ".ico" or ".heic" or ".heif";
    }

    /// <summary>
    /// Reads JPEGPhoto attribute from Directory Services (raw bytes, preferred method)
    /// </summary>
    private byte[]? TryReadDsclJpegPhoto(string userName, OsUserProfileResult result, System.Text.StringBuilder diagnosticInfo)
    {
        try
        {
            result.AttemptedPaths.Add($"dscl command: /usr/bin/dscl . -read /Users/{EscapeDsclArg(userName)} JPEGPhoto");
            
            var output = RunDsclCommand($". -read /Users/{EscapeDsclArg(userName)} JPEGPhoto", diagnosticInfo);
            if (string.IsNullOrWhiteSpace(output) || output.IndexOf("No such key", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                diagnosticInfo.AppendLine("   ⚠️ JPEGPhoto not found in directory record");
                return null;
            }

            diagnosticInfo.AppendLine($"   dscl output length: {output.Length} characters");
            
            // Extract all 0x........ tokens and convert to bytes
            var matches = Regex.Matches(output, @"0x[0-9A-Fa-f]{1,8}");
            if (matches.Count == 0)
            {
                diagnosticInfo.AppendLine("   ⚠️ No hex data found in JPEGPhoto output");
                return null;
            }

            diagnosticInfo.AppendLine($"   Found {matches.Count} hex words");
            
            using var ms = new MemoryStream();
            foreach (Match m in matches)
            {
                var hex = m.Value.Substring(2); // strip 0x
                // dscl may emit 1..8 hex digits; left-pad to even length for byte parsing
                if (hex.Length % 2 == 1) hex = "0" + hex;
                if (hex.Length < 8) hex = hex.PadLeft(8, '0'); // treat as 32-bit word

                // dscl prints 32-bit words in big-endian order
                for (int i = 0; i < hex.Length; i += 2)
                {
                    ms.WriteByte(Convert.ToByte(hex.Substring(i, 2), 16));
                }
            }

            var bytes = ms.ToArray();
            diagnosticInfo.AppendLine($"   Decoded {bytes.Length} bytes from hex data");

            // Basic sanity check for JPEG SOI marker (JPEG starts with FF D8)
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xD8)
            {
                diagnosticInfo.AppendLine("   ✅ Valid JPEG header detected (FF D8)");
                return bytes;
            }

            // Check for PNG header (89 50 4E 47)
            if (bytes.Length >= 4 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
            {
                diagnosticInfo.AppendLine("   ✅ Valid PNG header detected (89 50 4E 47)");
                return bytes;
            }

            // If it isn't a recognized header, still return it—caller can attempt image decoding
            diagnosticInfo.AppendLine($"   ⚠️ Unrecognized image format (first bytes: {BitConverter.ToString(bytes.Take(Math.Min(8, bytes.Length)).ToArray())})");
            return bytes;
        }
        catch (Exception ex)
        {
            diagnosticInfo.AppendLine($"   ❌ Error reading JPEGPhoto: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Reads Picture attribute from Directory Services (file path, fallback method)
    /// </summary>
    private string? TryReadDsclPicturePath(string userName, OsUserProfileResult result, System.Text.StringBuilder diagnosticInfo)
    {
        try
        {
            result.AttemptedPaths.Add($"dscl command: /usr/bin/dscl . -read /Users/{EscapeDsclArg(userName)} Picture");
            
            var output = RunDsclCommand($". -read /Users/{EscapeDsclArg(userName)} Picture", diagnosticInfo);
            if (string.IsNullOrWhiteSpace(output) || output.IndexOf("No such key", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                diagnosticInfo.AppendLine("   ⚠️ Picture attribute not found in directory record");
                return null;
            }

            // Find "Picture:" line and return rest
            var lines = output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                var trimmed = lines[i].Trim();
                if (trimmed.StartsWith("Picture:", StringComparison.OrdinalIgnoreCase))
                {
                    var value = trimmed.Substring("Picture:".Length).Trim();
                    if (string.IsNullOrEmpty(value) && i + 1 < lines.Length)
                    {
                        // Path is on the next line
                        value = lines[i + 1].Trim();
                    }
                    
                    if (!string.IsNullOrEmpty(value))
                    {
                        diagnosticInfo.AppendLine($"   Found Picture path: {value}");
                        result.FoundPaths.Add(value);
                        return value;
                    }
                }
            }

            diagnosticInfo.AppendLine("   ⚠️ Picture attribute found but no path extracted");
            return null;
        }
        catch (Exception ex)
        {
            diagnosticInfo.AppendLine($"   ❌ Error reading Picture attribute: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Runs dscl command and captures stdout
    /// </summary>
    private string RunDsclCommand(string arguments, System.Text.StringBuilder diagnosticInfo)
    {
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "/usr/bin/dscl",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            StandardErrorEncoding = System.Text.Encoding.UTF8,
        };

        using var p = System.Diagnostics.Process.Start(psi);
        if (p == null)
        {
            diagnosticInfo.AppendLine("   ❌ Failed to start dscl process");
            return string.Empty;
        }

        var stdout = p.StandardOutput.ReadToEnd();
        var stderr = p.StandardError.ReadToEnd();
        p.WaitForExit();

        diagnosticInfo.AppendLine($"   dscl exit code: {p.ExitCode}");
        if (!string.IsNullOrEmpty(stderr))
        {
            diagnosticInfo.AppendLine($"   dscl stderr: {stderr.Trim()}");
        }

        // dscl uses non-zero exit codes for missing keys/users; treat as "not found"
        if (p.ExitCode != 0)
        {
            return string.Empty;
        }

        return stdout;
    }

    /// <summary>
    /// Escapes arguments for dscl command
    /// </summary>
    private string EscapeDsclArg(string value)
    {
        // Conservative: dscl paths for local users typically have no spaces, but protect anyway
        return value.Replace("\\", "\\\\").Replace(" ", "\\ ");
    }
}
