VERSION = "1.4.9"
require "raykit"

task :env do
  start_task :env
  show_value "PROJECT.name", "#{PROJECT.version}"
  show_value "PROJECT.version", "#{PROJECT.version}"
end

task :build => [:env] do
  start_task :build
  try "rufo ."
  Raykit::Version::set_version_in_glob("**/*.csproj", VERSION)
  run("nuget restore #{PROJECT.name}.sln")
  run("dotnet build #{PROJECT.name}.sln --configuration Release")
end

task :test => [:build] do
  start_task :test
  #run("dotnet test #{PROJECT.name}.sln --configuration Release")
  run("dotnet test Node.Net.Test/Node.Net.Test.csproj -c Release")
end

task :tag => [:test] do
  start_task :tag
  if ENV["CI_SERVER"].nil?
    if GIT_DIRECTORY.has_tag PROJECT.version
      puts "git tag #{PROJECT.version} already exists"
    else
      puts "git tag #{PROJECT.version} does not exist"
      if (!PROJECT.read_only?)
        run("git integrate")
        run("git tag #{PROJECT.version} -m\"#{PROJECT.version}\"")
        run("git push --tags")
      end
    end
  else
    puts "CI_SERVER, skipping tag command"
  end
end

task :publish => [:tag] do
  start_task :publish
  if ENV["CI_SERVER"].nil?
    nuget = PROJECT.get_dev_dir("nuget")
    package = "#{PROJECT.name}/bin/Release/#{PROJECT.name}.#{PROJECT.version}.nupkg"
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

task :default => [:integrate, :publish, :push] do
  if (!PROJECT.read_only?)
    run("git pull")
  end
  puts "completed in #{PROJECT.elapsed}"
end
