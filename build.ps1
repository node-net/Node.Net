##########################################################################
# This is the Cake bootstrapper script for PowerShell.
# This file was downloaded from https://github.com/cake-build/resources
# Feel free to change this file to fit your needs.
##########################################################################

$CAKE_VERSION = "3.1.0"

<#
.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.

.DESCRIPTION
This Powershell script will download NuGet if missing, restore NuGet tools (including Cake)
and execute your Cake build script with the parameters you provide.

.PARAMETER Script
The build script to execute.
.PARAMETER Target
The build target to run.
.PARAMETER Configuration
The build configuration to use.
.PARAMETER Verbosity
Specifies the amount of information to be displayed.
.PARAMETER ShowDescription
Shows description about tasks.
.PARAMETER DryRun
Performs a dry run.
.PARAMETER Experimental
Uses the experimental Roslyn compiler.
.PARAMETER Mono
Uses the Mono scripting engine.
.PARAMETER SkipToolPackageRestore
Skips restoring of tools.
.PARAMETER ScriptArgs
Remaining arguments are added here.

.LINK
https://cakebuild.net
#>

[CmdletBinding()]
Param(
    [string]$Script = "build.cake",
    [string]$Target = "Default",
    [string]$Configuration = "Release",
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity = "Normal",
    [switch]$ShowDescription,
    [switch]$DryRun,
    [switch]$Experimental,
    [switch]$Mono,
    [switch]$SkipToolPackageRestore,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

# Attempt to set highest encryption available for SecurityProtocol.
# PowerShell will not set this by default (until maybe .NET 4.6.x). This
# will typically produce a message for PowerShell v2 (just an error
# message), for v3+ it will produce a warning.
# Try a combination of successfully supported protocols.
$supportedProtocols = @()
$protocols = [System.Net.SecurityProtocolType]::Tls12 -bor [System.Net.SecurityProtocolType]::Tls13
try {
    [System.Net.ServicePointManager]::SecurityProtocol = $protocols
    $supportedProtocols += [System.Net.SecurityProtocolType]::Tls12 -bor [System.Net.SecurityProtocolType]::Tls13
} catch {
    # Ignore
}

$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"
$CAKE_EXE = Join-Path $TOOLS_DIR "Cake/Cake.exe"
$NUGET_URL = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$PACKAGES_CONFIG = Join-Path $TOOLS_DIR "packages.config"
$PACKAGES_CONFIG_MD5 = Join-Path $TOOLS_DIR "packages.config.md5sum"

# Should we use mono?
$UseMono = "";
if ($Mono.IsPresent) {
    Write-Verbose -Message "Explicitly requested Mono usage."
    $UseMono = "-mono"
}

# Should we use the new Roslyn?
$UseExperimental = "";
if ($Experimental.IsPresent -and !($Mono.IsPresent)) {
    Write-Verbose -Message "Explicitly requested experimental compiler."
    $UseExperimental = "-experimental"
}

# Is this a dry run?
$UseDryRun = "";
if ($DryRun.IsPresent) {
    Write-Verbose -Message "Explicitly requested dry run."
    $UseDryRun = "-dryrun"
}

# Should we show description?
$UseShowDescription = "";
if ($ShowDescription.IsPresent) {
    Write-Verbose -Message "Explicitly requested show description."
    $UseShowDescription = "-showdescription"
}

# Make sure tools folder exists
if ((Test-Path $PSScriptRoot)) {
    if (!(Test-Path $TOOLS_DIR)) {
        Write-Verbose -Message "Creating tools directory..."
        New-Item -Path $TOOLS_DIR -Type directory | Out-Null
    }
} else {
    Write-Error -Message "Could not find the script root directory."
    exit 1
}

# Make sure that packages.config exist.
if (!(Test-Path $PACKAGES_CONFIG)) {
    Write-Verbose -Message "Downloading packages.config..."
    try {
        $NuGetContent = "<?xml version=`"1.0`" encoding=`"utf-8`"?>`n<packages>`n  <package id=`"Cake`" version=`"$CAKE_VERSION`" />`n</packages>"
        $NuGetContent | Out-File -FilePath $PACKAGES_CONFIG -Encoding "utf8"
    } catch {
        Throw "Could not create packages.config."
    }
}

# Try find NuGet.exe in path if not exists
if (!(Test-Path $NUGET_EXE)) {
    Write-Verbose -Message "Trying to find nuget.exe in PATH..."
    $existingPaths = $Env:Path -Split ';' | Where-Object { $_ -ne '' }
    $NUGET_IN_PATH = Get-ChildItem -Path $existingPaths -Filter "nuget.exe" -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($NUGET_IN_PATH -ne $null -and (Test-Path $NUGET_IN_PATH.FullName)) {
        Write-Verbose -Message "Found in PATH at $($NUGET_IN_PATH.FullName)."
        $NUGET_EXE = $NUGET_IN_PATH.FullName
    }
}

# Try download NuGet.exe if not exists
if (!(Test-Path $NUGET_EXE)) {
    Write-Verbose -Message "Downloading NuGet.exe..."
    try {
        [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
        (New-Object System.Net.WebClient).DownloadFile($NUGET_URL, $NUGET_EXE)
    } catch {
        Throw "Could not download NuGet.exe."
    }
}

# Save nuget.exe path to environment to be available to child processes
$ENV:NUGET_EXE = $NUGET_EXE
$NUGET_EXE_INVOCATION = if ($IsLinux -or $IsMacOS) {
    "mono `"$NUGET_EXE`""
} else {
    "`"$NUGET_EXE`""
}

# Restore tools from NuGet?
if (-Not $SkipToolPackageRestore.IsPresent) {
    Push-Location
    Set-Location $TOOLS_DIR

    # Check for changes in packages.config and remove installed tools if true.
    [string]$md5Hash = ""
    if (Test-Path $PACKAGES_CONFIG_MD5) {
        $md5Hash = [System.IO.File]::ReadAllText($PACKAGES_CONFIG_MD5)
    }
    [string]$newMD5Hash = ""
    if (Test-Path $PACKAGES_CONFIG) {
        $utf8NoBom = New-Object System.Text.UTF8Encoding $false
        $bytes = [System.IO.File]::ReadAllBytes($PACKAGES_CONFIG)
        $newMD5Hash = [System.Security.Cryptography.MD5]::Create().ComputeHash($bytes) | ForEach-Object { $_.ToString("x2") }
    }

    if (($md5Hash -ne $newMD5Hash) -Or !(Test-Path $CAKE_EXE)) {
        Write-Verbose -Message "Restoring tools from NuGet..."
        $NuGetOutput = Invoke-Expression "& $NUGET_EXE_INVOCATION install `"$PACKAGES_CONFIG`" -ExcludeVersion -OutputDirectory `"$TOOLS_DIR`""

        if ($LASTEXITCODE -ne 0) {
            Throw "An error occurred while restoring NuGet tools."
        }
    } else {
        Write-Verbose -Message "Skipping NuGet tools restore..."
    }
    Write-Verbose -Message ($md5Hash | Out-String)
    $newMD5Hash | Out-File -FilePath $PACKAGES_CONFIG_MD5 -Encoding "utf8" -NoNewline

    Pop-Location
} else {
    Write-Verbose -Message "Skipping NuGet tools restore..."
}

# Make sure that Cake has been installed.
if (!(Test-Path $CAKE_EXE)) {
    Throw "Could not find Cake.exe at $CAKE_EXE. Use -SkipToolPackageRestore if you want to skip this step."
}

# Build Cake arguments
$cakeArguments = @("`"$Script`"");
$cakeArguments += "--target=`"$Target`"";
$cakeArguments += "--configuration=`"$Configuration`"";
$cakeArguments += "--verbosity=`"$Verbosity`"";

if ($UseMono) {
    $cakeArguments += $UseMono;
}
if ($UseExperimental) {
    $cakeArguments += $UseExperimental;
}
if ($UseDryRun) {
    $cakeArguments += $UseDryRun;
}
if ($UseShowDescription) {
    $cakeArguments += $UseShowDescription;
}
$cakeArguments += $ScriptArgs

# Start Cake
Write-Host "Running build script..." -ForegroundColor Green
& "$CAKE_EXE" $cakeArguments
exit $LASTEXITCODE
