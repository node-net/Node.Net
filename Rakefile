VERSION = "2.0.11"
#require "raykit"
require "makit"
require_relative "scripts/ruby/makit/github_actions"
#require_relative "scripts/ruby/makit/nuget_ext"

task :default => [:setup, :build, :test, :integrate, :tag, :publish, :pull_incoming, :sync]

task :build do
  puts `rufo .`
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
      Makit::NuGet::publish(package, Makit::Secrets.get("nuget_api_key"), "https://api.nuget.org/v3/index.json")
    else
      puts "nuget_api_key SECRET not available"
    end
    #if ENV["CI_SERVER"].nil?

    #if (!File.exist?("#{nuget}/#{PROJECT.name}.#{PROJECT.version}.nupkg"))
    #  FileUtils.cp(package, "#{nuget}/#{PROJECT.name}.#{PROJECT.version}.nupkg")
    #end
    #if (SECRETS.has_key?("nuget_api_key"))
    #  Makit::NuGet::publish(package, SECRETS["nuget_api_key"], "https://api.nuget.org/v3/index.json")
    #else
    #  puts "nuget_api_key SECRET not available"
    # end
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

  if (!Makit::Secrets.has_key?("github_token"))
    puts "github_token SECRET not available (optional - for GitHub Actions status queries)"
    # Optionally prompt for it, or leave it to be set manually
    Makit::Secrets.set("github_token") # prompt for the key
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

def get_github_repo_info
  remote_url = `git config --get remote.origin.url`.strip
  if remote_url.empty?
    raise "Could not determine git remote URL. Make sure you're in a git repository with a remote configured."
  end

  # Handle both https:// and git@ formats
  match = remote_url.match(%r{github\.com[:/]([^/]+)/([^/]+)(?:\.git)?$})
  if match
    owner = match[1]
    repo = match[2].gsub(/\.git$/, "")
    return [owner, repo]
  else
    raise "Could not parse GitHub repository from remote URL: #{remote_url}"
  end
end

desc "Query GitHub Actions workflow status"
task :actions_status do
  begin
    owner, repo = get_github_repo_info
    branch = `git rev-parse --abbrev-ref HEAD`.strip
    branch = "main" if branch.empty?

    puts "Querying GitHub Actions status for #{owner}/#{repo} (branch: #{branch})..."

    if (Makit::Secrets.has_key?("github_token"))
      token = Makit::Secrets.get("github_token")
      result = Makit::GitHubActions::workflow_status(owner, repo, branch: branch, token: token)
    else
      puts "github_token SECRET not available"
      result = Makit::GitHubActions::workflow_status(owner, repo, branch: branch)
    end

    if result[:status] == "not_found"
      puts "⚠️  #{result[:message]}"
    else
      status_emoji = case result[:conclusion]
        when "success"
          "✅"
        when "failure"
          "❌"
        when "cancelled"
          "🚫"
        when nil
          result[:status] == "completed" ? "⏸️" : "🔄"
        else
          "❓"
        end

      puts "\n#{status_emoji} Workflow Status: #{result[:workflow_name] || "Unknown"}"
      puts "   Status: #{result[:status]}"
      puts "   Conclusion: #{result[:conclusion] || "N/A"}" if result[:conclusion]
      puts "   Run Number: ##{result[:run_number]}" if result[:run_number]
      puts "   Created: #{result[:created_at]}" if result[:created_at]
      puts "   Updated: #{result[:updated_at]}" if result[:updated_at]
      puts "   URL: #{result[:html_url]}" if result[:html_url]
    end
  rescue => e
    puts "❌ Error: #{e.message}"
    exit 1
  end
end
