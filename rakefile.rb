VERSION='1.2.56'
SLN_FILES=FileList.new('Node.Net.NETFramework4.6.sln')
require 'dev'
CLOBBER.include('**/obj','bin','TestResults')

rake_dir=File.dirname(__FILE__)
runner="#{rake_dir}/packages/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe"
opencover="#{rake_dir}/packages/OpenCover.4.6.519/tools/OpenCover.Console.exe"

task :analyze => [:test] do
	Dir.chdir('bin/Release/net46') do
		puts `SonarScanner.MSBuild.exe begin /k:"node_net" /d:sonar.organization="node-net" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="17b418d5e80f50397e9a8bc688446a59c08adc2b" /d:sonar.cs.opencover.reportsPaths="coverage.opencover.xml" /v:"#{VERSION}"`
		puts `MsBuild.exe /t:Rebuild ../../../Node.Net.NETFramework4.6.sln /p:Configuration=Release`
		puts `#{opencover} -target:#{runner} -targetargs:"Node.Net.Test.dll" -register:user -output:coverage.opencover.xml -filter:"+[Node.Net.*]*"`
		puts `SonarScanner.MSBuild.exe end /d:sonar.login="17b418d5e80f50397e9a8bc688446a59c08adc2b"`
	end
end

task :coverage do
	Dir.chdir('bin/Release/net46') do
		puts `#{opencover} -target:#{runner} -targetargs:"Node.Net.Test.dll" -register:user -output:coverage.opencover.xml -filter:"+[Node.Net.*]*"`
	end
end

#task :package => [:build] do
#	puts `nuget pack Node.Net/Node.Net.csproj -Symbols -SymbolPackageFormat snupkg`
#end

task :publish  do
	list=`nuget list Node.Net -Source nuget.org`
	if(!list.include?("Node.Net #{VERSION}"))
		puts `nuget push Node.Net.#{VERSION}.nupkg -Source https://api.nuget.org/v3/index.json`
	end
end

#task :default => [:analyze]