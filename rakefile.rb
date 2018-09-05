VERSION='1.2.21'
require 'dev'
CLOBBER.include('**/obj','bin','TestResults')

task :publish  do
	list=`nuget list Node.Net -Source nuget.org`
	if(!list.include?("Node.Net #{VERSION}"))
		puts `nuget push Node.Net.#{VERSION}.nupkg -Source https://api.nuget.org/v3/index.json`
	end
end
