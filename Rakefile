VERSION = "2.0.11"
require "raykit"
require "makit"

task :default => [:integrate, :publish, :pull_incoming, :sync] do
  #if (!PROJECT.read_only?)
  #  run("git pull")
  # end
  puts "completed in #{PROJECT.elapsed}"
end

task :build do
  start_task :build
  try "rufo ."
  #Raykit::Version::set_version_in_glob("**/*.csproj", VERSION)
  Makit::Version::set_version_in_files("**/*.csproj", VERSION)

  targets = compatible_targets
  if targets.empty?
    # Build all targets (Windows)
    run("dotnet restore #{PROJECT.name}.sln")
    run("dotnet build #{PROJECT.name}.sln --configuration Release")
  else
    # Build only compatible targets (Mac/Linux)
    run("dotnet restore source/Node.Net/Node.Net.csproj #{targets}")
    run("dotnet build source/Node.Net/Node.Net.csproj --configuration Release #{targets}")
    run("dotnet restore tests/Node.Net.Test/Node.Net.Test.csproj #{targets}")
    run("dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --configuration Release #{targets}")
  end
end

task :test => [:build] do
  start_task :test
  targets = compatible_targets
  if targets.empty?
    # Test all targets (Windows)
    run("dotnet test tests/Node.Net.Test/Node.Net.Test.csproj -c Release")
  else
    # Test only compatible targets (Mac/Linux)
    run("dotnet test tests/Node.Net.Test/Node.Net.Test.csproj -c Release #{targets}")
  end
end

task :tag => [:test] do
  start_task :tag
  if ENV["CI_SERVER"].nil?
    if GIT_DIRECTORY.has_tag PROJECT.version
      puts "git tag #{PROJECT.version} already exists"
    else
      puts "git tag #{PROJECT.version} does not exist"
      if (!PROJECT.read_only?)
        run("git add .")
        run("git tag #{PROJECT.version} -m\"#{PROJECT.version}\"")
        run("git push --tags")
      end
    end
  else
    puts "CI_SERVER, skipping tag command"
  end
end

desc "run the examples/Node.Net.AspNet.Host project"
task :run => [:test] do
  start_task :run
  sh "dotnet run --project examples/Node.Net.AspNet.Host/Node.Net.AspNet.Host.csproj"
end

task :publish => [:tag] do
  start_task :publish
  if ENV["CI_SERVER"].nil?
    nuget = PROJECT.get_dev_dir("nuget")
    package = "source/Node.Net/bin/Release/#{PROJECT.name}.#{PROJECT.version}.nupkg"
    if (!File.exist?("#{nuget}/#{PROJECT.name}.#{PROJECT.version}.nupkg"))
      FileUtils.cp(package, "#{nuget}/#{PROJECT.name}.#{PROJECT.version}.nupkg")
    end
    if (SECRETS.has_key?("nuget_api_key"))
      run("dotnet nuget push #{package} --skip-duplicate --api-key #{SECRETS["nuget_api_key"]} --source https://api.nuget.org/v3/index.json")
    else
      puts "nuget_api_key SECRET not available"
    end
  else
    puts "CI_SERVER, skipping publish command"
  end
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
