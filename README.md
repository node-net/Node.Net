# Node.Net
[![NuGet Version and Downloads count](https://buildstats.info/nuget/Node.Net)](https://www.nuget.org/packages/Node.Net)
![license](https://img.shields.io/github/license/node-net/Node.Net.svg)
Node.Net is a framework library to support IDictionary data operations, including serialization to and from JSON

## Table of Contents ##

- [Downloads](#downloads)
- [Cross-Platform Compatibility](#cross-platform-compatibility)
- [GitHub Actions Setup](#github-actions-setup)
- [License](#license)

## Downloads ##

The latest release of Node.Net is [available on NuGet](https://www.nuget.org/packages/Node.Net/).

## Cross-Platform Compatibility ##

Node.Net supports multiple target frameworks:
- **Windows:** `net48`, `net8.0-windows` (uses framework-provided WPF types)
- **Cross-Platform:** `net8.0`, `net8.0-wasm`, `netstandard2.0` (provides `System.Windows.*` type implementations)
- **.NET Standard 2.0:** `netstandard2.0` (compatible with .NET Framework 4.6.1+, .NET Core 2.0+, and other platforms)

For detailed information about how `System.Windows.*` types are conditionally compiled and used across different target frameworks, see [docs/SYSTEM_NAMESPACE_RULES.md](docs/SYSTEM_NAMESPACE_RULES.md).

## GitHub Actions Setup ##

Node.Net uses GitHub Actions to automatically build, test, and publish packages to NuGet.org. To enable automatic publishing, you need to configure the NuGet API key secret.

### Setting Up NuGet API Key Secret

1. **Get your NuGet API key:**
   - Go to [NuGet.org API Keys](https://www.nuget.org/account/apikeys)
   - Create a new API key (or use an existing one)
   - Copy the API key value

2. **Add the secret to GitHub:**
   - Navigate to your repository on GitHub
   - Go to **Settings** → **Secrets and variables** → **Actions**
   - Click **New repository secret**
   - Name: `NUGET_API_KEY`
   - Value: Paste your NuGet API key
   - Click **Add secret**

3. **Verify the workflow:**
   - The workflow (`.github/workflows/dotnetcore.yml`) is already configured
   - It will automatically publish when:
     - Pushing to the `main` branch
     - Pushing a version tag (e.g., `v2.0.12`)
   - The workflow uses `--skip-duplicate` flag, so it won't fail if the version already exists

For more detailed information about the GitHub Actions workflow, see [docs/github-actions-nuget-publish.md](docs/github-actions-nuget-publish.md).

## License ##

Node.Net is Open Source software and is released under the [MIT license](https://github.com/node-net/Node.Net/wiki/License).