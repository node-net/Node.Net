NAME="Node.Net"
require 'raykit'
CLEAN.include('**/obj')

task :default do
	PROJECT.info
    target='tmp.txt'
    CLEAN.exclude(target)
    if(PROJECT.last_modified_filename != target)
        PROJECT.run(["dotnet build --configuration Release",
                    "dotnet test #{PROJECT.name}.Test/#{PROJECT.name}.Test.csproj -c Release -v normal",
                    "dotnet pack #{PROJECT.name}.sln -c Release"])

        package="#{PROJECT.name}/bin/Release/#{PROJECT.name}.#{PROJECT.version}.nupkg"
		puts "publishing" + Rainbow(package).yellow.bright + " to " + Rainbow("nuget.org").yellow.bright
		NUGET_KEY=ENV['NUGET_KEY']
		puts `nuget push #{package} -ApiKey #{NUGET_KEY} -Source https://api.nuget.org/v3/index.json`
        
        PROJECT.commit.tag.push.pull
    end
    PROJECT.summary
end

task :setup do
    Raykit::DotNet::initialize_csharp_lib PROJECT.name
end
