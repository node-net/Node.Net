VERSION='1.1.22'
#SLN_FILES=FileList.new('Node.Net.NETStandard2.0.sln','Node.Net.NETFramework4.6.sln')
require 'dev'
CLOBBER.include('**/obj','bin','TestResults')

task :publish  do
	list=`nuget list Node.Net -Source VSTS -Prerelease`
	if(!list.include?("Node.Net #{VERSION}"))
	#	# Have VSTS package and push the NuGet Packages
	#	puts `nuget push Ouroboros.Reflection.#{VERSION}.nupkg -Source "https://lou-parslow.pkgs.visualstudio.com/_packaging/Packages/nuget/v3/index.json" -ApiKey VSTS`
		puts `nuget push Node.Net.#{VERSION}-beta.nupkg -Source https://api.nuget.org/v3/index.json`
	end
end
