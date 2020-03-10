NAME="Node.Net"
require 'raykit'
CLEAN.include('**/obj')

task :integrate do
	PROJECT.info
    SECRETS['NUGET_KEY'] = ENV['NUGET_KEY']
    package="#{PROJECT.name}/bin/Release/#{PROJECT.name}.#{PROJECT.version}.nupkg"
    target='tmp.txt'
    CLEAN.exclude(target)
    if(PROJECT.last_modified_filename != target)
        PROJECT.run(["dotnet build --configuration Release",
                    "dotnet test #{PROJECT.name}.Test/#{PROJECT.name}.Test.csproj -c Release -v normal",
                    "dotnet pack #{PROJECT.name}.sln -c Release",
                    "nuget push #{package} -SkipDuplicate -ApiKey #{SECRETS['NUGET_KEY']} -Source https://api.nuget.org/v3/index.json"])
    
        #PROJECT.commit.tag.push.pull
        PROJECT.run("git add --all")
        PROJECT.run("git commit -m\"integrate\"") if(PROJECT.outstanding_commit?)
        PROJECT.run("git tag #{PROJECT.version} -m'#{PROJECT.version}'") if(PROJECT.latest_tag != PROJECT.version)
        PROJECT.run("git push")
        PROJECT.run("git push --tags")
        PROJECT.run("git pull")
    end
    PROJECT.summary
end

desc 'default task'
task :default => [:integrate]

desc 'bump the version'
task :bump do
    puts Rainbow(':bump').blue.bright
    old_version = Raykit::Version.detect(PROJECT.name)
    new_version = Raykit::Version.bump_file("#{PROJECT.name}/#{PROJECT.name}.csproj")
    puts "        bumped version from #{old_version} to #{new_version}"
    puts "        perform rake :integrate to integrate this change."
    puts ''
    
end

task :fix do
    analyzer_dir="#{Raykit::Environment::local_application_data}/Microsoft/VisualStudio"
    PROJECT.run("roslynator fix #{PROJECT.name}.sln --analyzer-assemblies #{analyzer_dir} --msbuild-path \"#{Raykit::MsBuild::msbuild_path}\"")
end
