# GitHub Actions NuGet Publishing Setup

This guide explains how to set up GitHub Actions to automatically publish NuGet packages with duplicate version handling.

## Overview

The workflow (`.github/workflows/dotnetcore.yml`) is configured to:
- ✅ Build and test on every push and pull request
- ✅ Publish to NuGet only on pushes to `main` branch or version tags (`v*`)
- ✅ Skip publishing if the package version already exists (no error)

## Setup Instructions

### 1. Add NuGet API Key Secret

1. Go to your GitHub repository
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Name: `NUGET_API_KEY`
5. Value: Your NuGet API key (get it from https://www.nuget.org/account/apikeys)
6. Click **Add secret**

### 2. Verify Workflow Configuration

The workflow is already configured in `.github/workflows/dotnetcore.yml` with:

```yaml
- name: Publish to NuGet
  if: github.event_name == 'push' && (github.ref == 'refs/heads/main' || startsWith(github.ref, 'refs/tags/v'))
  run: |
    $package = Get-ChildItem -Path "artifacts/nupkg/*.nupkg" | Select-Object -First 1
    if ($package) {
      dotnet nuget push $package.FullName `
        --skip-duplicate `
        --api-key $env:NUGET_API_KEY `
        --source https://api.nuget.org/v3/index.json
    }
  env:
    NUGET_API_KEY: ${{ secrets.NUGET_API_KEY }}
```

### 3. How It Works

#### Skip Duplicate Behavior

The `--skip-duplicate` flag tells NuGet to:
- ✅ **Skip silently** if the package version already exists on NuGet.org
- ✅ **Publish** if the version doesn't exist
- ✅ **No error** if duplicate is detected (workflow continues successfully)

#### When Publishing Happens

The workflow publishes when:
- ✅ Push to `main` branch
- ✅ Push of a tag starting with `v` (e.g., `v2.0.12`)

The workflow does **NOT** publish on:
- ❌ Pull requests
- ❌ Pushes to other branches
- ❌ Tags not starting with `v`

### 4. Testing the Setup

#### Test Build (No Publish)
```bash
# Push to a feature branch - will build/test but not publish
git checkout -b test-branch
git push origin test-branch
```

#### Test Publish (Main Branch)
```bash
# Push to main - will build/test AND publish
git checkout main
git push origin main
```

#### Test Publish (Tag)
```bash
# Create and push a version tag - will build/test AND publish
git tag v2.0.12
git push origin v2.0.12
```

### 5. Version Management

The package version comes from:
- `Rakefile` → `VERSION = "2.0.12"`
- `source/Node.Net/Node.Net.csproj` → `<Version>2.0.12</Version>`

The Rakefile task `:build` automatically updates version in all `.csproj` files.

### 6. Workflow Steps

1. **Checkout** - Gets the code
2. **Setup .NET** - Installs .NET SDK 8.0.122 (matches `global.json`)
3. **Restore** - Restores NuGet packages
4. **Build** - Builds the solution in Release mode
5. **Package** - Creates `.nupkg` file in `artifacts/nupkg/`
6. **Test** - Runs unit tests
7. **Publish** - Publishes to NuGet (only on main/tags, with skip-duplicate)

### 7. Troubleshooting

#### Workflow Fails on Publish Step

**Check:**
- ✅ `NUGET_API_KEY` secret is set in GitHub repository settings
- ✅ API key is valid and not expired
- ✅ API key has permissions to publish packages

#### Package Not Publishing

**Check:**
- ✅ Are you pushing to `main` branch or a `v*` tag?
- ✅ Check workflow logs for any errors
- ✅ Verify the package file exists in `artifacts/nupkg/`

#### Duplicate Version Error

**Note:** With `--skip-duplicate`, you should **NOT** see duplicate errors. If you do:
- Check NuGet.org to see if the version already exists
- The workflow should skip silently, not error

### 8. Manual Publishing (Fallback)

If you need to publish manually:

```powershell
# Build and package
dotnet pack source/Node.Net/Node.Net.csproj -c Release -o artifacts/nupkg

# Publish with skip-duplicate
dotnet nuget push artifacts/nupkg/Node.Net.2.0.12.nupkg `
  --skip-duplicate `
  --api-key YOUR_API_KEY `
  --source https://api.nuget.org/v3/index.json
```

## Related Files

- `.github/workflows/dotnetcore.yml` - GitHub Actions workflow
- `Rakefile` - Version management and local publish task
- `scripts/ruby/makit/nuget_ext.rb` - Ruby helper (also uses `--skip-duplicate`)
- `source/Node.Net/Node.Net.csproj` - Package metadata

## Security Notes

- ✅ API key is stored as a GitHub Secret (encrypted)
- ✅ API key is only exposed in the publish step
- ✅ Never commit API keys to the repository
- ✅ Use repository secrets, not environment secrets (unless needed)
