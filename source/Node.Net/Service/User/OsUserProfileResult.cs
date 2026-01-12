using System.Collections.Generic;

namespace Node.Net.Service.User;

/// <summary>
/// Result of attempting to get OS user profile picture
/// </summary>
public class OsUserProfileResult
{
    public string? ProfilePictureUrl { get; set; }
    public List<string> AttemptedPaths { get; set; } = new();
    public List<string> FoundPaths { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? DiagnosticInfo { get; set; }
}
