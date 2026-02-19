# Cake Build Script

This project includes a Cake (C# Make) build script for building and testing the solution.

## Prerequisites

- .NET SDK 8.0.122 or later (as specified in `global.json`)
- PowerShell (Windows) or Bash (Linux/macOS) for bootstrapping

## Quick Start

### Windows (PowerShell)

```powershell
# Run default build and test
.\build.ps1

# Run specific target
.\build.ps1 --target=Build
.\build.ps1 --target=Test
.\build.ps1 --target=CI

# Run with specific configuration
.\build.ps1 --configuration=Debug

# Show available tasks
.\build.ps1 --target=Default --showdescription
```

### Linux/macOS (Bash)

```bash
# Make script executable (first time only)
chmod +x build.sh

# Run default build and test
./build.sh

# Run specific target
./build.sh --target=Build
./build.sh --target=Test
./build.sh --target=CI

# Run with specific configuration
./build.sh --configuration=Debug
```

### Using dotnet cake directly

```bash
# Install Cake as a .NET tool (first time only)
dotnet tool install -g Cake.Tool

# Run build script
dotnet cake build.cake

# Run specific target
dotnet cake build.cake --target=Build
dotnet cake build.cake --target=Test
dotnet cake build.cake --target=CI

# Run with specific configuration
dotnet cake build.cake --configuration=Debug
```

## Available Tasks

### Default
Runs `Build` and `Test` tasks. This is the default target.

```bash
dotnet cake build.cake
# or
dotnet cake build.cake --target=Default
```

### Clean
Cleans build artifacts and output directories.

```bash
dotnet cake build.cake --target=Clean
```

### Restore
Restores NuGet packages for the solution.

```bash
dotnet cake build.cake --target=Restore
```

### Build
Restores packages and builds the solution in Release configuration.

```bash
dotnet cake build.cake --target=Build
```

### Pack
Builds the solution and creates a NuGet package in `artifacts/nupkg/`.

```bash
dotnet cake build.cake --target=Pack
```

### Test
Builds the solution and runs all tests.

```bash
dotnet cake build.cake --target=Test
```

Runs tests for:
- `Node.Net.Test`
- `Node.Net.Components.Test` (if present)

### CI
Runs a full CI pipeline: Clean → Build → Test → Pack.

```bash
dotnet cake build.cake --target=CI
```

## Configuration Options

### Configuration
Specify build configuration (Debug or Release, default: Release):

```bash
dotnet cake build.cake --configuration=Debug
```

### Verbosity
Control output verbosity (Quiet, Minimal, Normal, Verbose, Diagnostic):

```bash
dotnet cake build.cake --verbosity=Verbose
```

### Dry Run
See what tasks would run without executing them:

```bash
dotnet cake build.cake --dryrun
```

### Show Description
Display task descriptions:

```bash
dotnet cake build.cake --showdescription
```

## Examples

### Build and test in Debug mode
```bash
dotnet cake build.cake --configuration=Debug --target=Default
```

### Clean build and test
```bash
dotnet cake build.cake --target=CI
```

### Build only (no tests)
```bash
dotnet cake build.cake --target=Build
```

### Test only (assumes already built)
```bash
dotnet cake build.cake --target=Test
```

### Create package
```bash
dotnet cake build.cake --target=Pack
```

## Integration with CI/CD

The Cake script can be used in CI/CD pipelines:

### GitHub Actions
```yaml
- name: Build and Test
  run: dotnet cake build.cake --target=CI
```

### Azure DevOps
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Build and Test'
  inputs:
    command: 'custom'
    custom: 'cake'
    arguments: 'build.cake --target=CI'
```

## Troubleshooting

### Cake tool not found
Install Cake as a .NET tool:
```bash
dotnet tool install -g Cake.Tool
```

### NuGet restore fails
Ensure you have internet connectivity and NuGet.org is accessible. Check your NuGet configuration:
```bash
dotnet nuget list source
```

### Build fails on Windows
Ensure you're using the correct .NET SDK version as specified in `global.json`:
```bash
dotnet --version
```

### Tests fail
Ensure all dependencies are restored:
```bash
dotnet cake build.cake --target=Restore
dotnet cake build.cake --target=Test
```

## Files

- `build.cake` - Main Cake build script
- `build.ps1` - PowerShell bootstrapper (Windows)
- `build.sh` - Bash bootstrapper (Linux/macOS)
- `tools/` - Cake tools directory (auto-generated, gitignored)

## Related Documentation

- [Cake Documentation](https://cakebuild.net/docs)
- [Cake API Reference](https://cakebuild.net/api)
- [Rakefile](Rakefile) - Alternative Ruby-based build script
