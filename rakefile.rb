NAME="Node.Net"
require 'raykit'
CLEAN.include('**/obj')

task :default do
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
        
        PROJECT.commit.tag.push.pull
    end
    PROJECT.summary
end

task :fix do
    analyzer_dir="#{Raykit::Environment::local_application_data}/Microsoft/VisualStudio"
    PROJECT.run("roslynator fix #{PROJECT.name}.sln --analyzer-assemblies #{analyzer_dir} --msbuild-path \"#{Raykit::MsBuild::msbuild_path}\"")
end
