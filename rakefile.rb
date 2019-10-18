NAME="Node.Net"
require 'raykit'
CLEAN.include('**/obj')

task :default do
    #PROJECT.verbose=true
	PROJECT.info
    target='Quemulus.Standard.Test/obj/Quemulus.Standard.Test.csproj.nuget.cache'
    CLEAN.exclude(target)
    if(PROJECT.last_modified_filename != target)
        PROJECT.run(["dotnet build --configuration Release",
                    "dotnet test #{PROJECT.name}.Test/#{PROJECT.name}.Test.csproj -c Release -v normal",
                    "dotnet pack Node.Net.sln -c Release"])

        package="#{PROJECT.name}/bin/Release/#{PROJECT.name}.#{PROJECT.version}-alpha.nupkg"
        dest="#{PROJECT.get_dev_dir('nuget')}/#{PROJECT.name}.#{PROJECT.version}-alpha.nupkg"
        puts "copying " + Rainbow(package).yellow.bright + " to " + Rainbow(dest).yellow.bright
        FileUtils.cp(package,dest)

		package="#{PROJECT.name}.Windows/bin/Release/#{PROJECT.name}.Windows.#{PROJECT.version}-alpha.nupkg"
        dest="#{PROJECT.get_dev_dir('nuget')}/#{PROJECT.name}.Windows.#{PROJECT.version}-alpha.nupkg"
        puts "copying " + Rainbow(package).yellow.bright + " to " + Rainbow(dest).yellow.bright
        FileUtils.cp(package,dest)
        
        PROJECT.commit.tag.push.pull
    end
    PROJECT.summary
end

task :setup do
    Raykit::DotNet::initialize_csharp_lib PROJECT.name
end
