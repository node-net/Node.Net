VERSION = "2.0.11"
require "raykit"
require "makit"
require_relative "scripts/ruby/makit/nuget_ext"

task :default => [:setup, :build, :test, :integrate, :tag, :publish, :pull_incoming, :sync]

task :build do
  try "rufo ."
  #Raykit::Version::set_version_in_glob("**/*.csproj", VERSION)
  Makit::Version::set_version_in_files("**/*.csproj", VERSION)

  targets = compatible_targets
  if targets.empty?
    # Build all targets (Windows)
    sh "dotnet restore #{PROJECT.name}.sln"
    sh "dotnet build #{PROJECT.name}.sln --configuration Release"
    sh "dotnet pack source/Node.Net/Node.Net.csproj -c Release -o artifacts/nupkg"
  else
    # Build only compatible targets (Mac/Linux)
    sh "dotnet restore source/Node.Net/Node.Net.csproj #{targets}"
    sh "dotnet build source/Node.Net/Node.Net.csproj --configuration Release #{targets}"
    sh "dotnet restore tests/Node.Net.Test/Node.Net.Test.csproj #{targets}"
    sh "dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --configuration Release #{targets}"
  end
end

task :test => [:build] do
  targets = compatible_targets
  if targets.empty?
    # Test all targets (Windows)
    sh "dotnet test tests/Node.Net.Test/Node.Net.Test.csproj -c Release"
  else
    # Test only compatible targets (Mac/Linux)
    sh "dotnet test tests/Node.Net.Test/Node.Net.Test.csproj -c Release #{targets}"
  end
end

desc "run the examples/Node.Net.AspNet.Host project"
task :run => [:test] do
  sh "dotnet run --project examples/Node.Net.AspNet.Host/Node.Net.AspNet.Host.csproj"
end

task :publish => [:build, :tag] do
  if (Makit::Environment.is_windows?)
    sh "dotnet pack source/Node.Net/Node.Net.csproj -c Release -o artifacts/nupkg"
    nuget = PROJECT.get_dev_dir("nuget")
    package = "source/Node.Net/bin/Release/#{PROJECT.name}.#{PROJECT.version}.nupkg"

    if (Makit::Secrets.has_key?("nuget_api_key"))
      Makit::NuGetExt::publish(package, Makit::Secrets::get_key("nuget_api_key"), "https://api.nuget.org/v3/index.json")
    else
      puts "nuget_api_key SECRET not available"
    end
    #if ENV["CI_SERVER"].nil?

    if (!File.exist?("#{nuget}/#{PROJECT.name}.#{PROJECT.version}.nupkg"))
      FileUtils.cp(package, "#{nuget}/#{PROJECT.name}.#{PROJECT.version}.nupkg")
    end
    if (SECRETS.has_key?("nuget_api_key"))
      Makit::NuGetExt::publish(package, SECRETS["nuget_api_key"], "https://api.nuget.org/v3/index.json")
    else
      puts "nuget_api_key SECRET not available"
    end
    #else
    #puts "CI_SERVER, skipping publish command"
    #end
  else
    puts "not windows, skipping publish command"
  end
end

task :setup do
  # secrets management
  #if (SECRETS.has_key?("nuget_api_key"))
  #  Makit::Secrets.set("nuget_api_key", SECRETS["nuget_api_key"])
  if (!Makit::Secrets.has_key?("nuget_api_key"))
    puts "nuget_api_key SECRET not available"
    Makit::Secrets.set("nuget_api_key") # prompt for the key
  end

  sh "dotnet new razorclasslib -n Node.Net.Components -o source/Node.Net.Components" unless Dir.exist?("source/Node.Net.Components")
  sh "dotnet new TUnit -n Node.Net.Components.Test -o tests/Node.Net.Components.Test" unless Dir.exist?("tests/Node.Net.Components.Test")
end

# Detect platform for cross-platform builds
def is_windows?
  RbConfig::CONFIG["host_os"] =~ /mswin|mingw|cygwin/
end

def compatible_targets
  if is_windows?
    # On Windows, build all targets
    ""
  else
    # On Mac/Linux, only build non-Windows targets
    "/p:TargetFrameworks=net8.0"
  end
end
