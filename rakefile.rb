VERSION='1.0.525'
require 'dev'

CLEAN.include(".sonarqube")
task :publish  do
	if(__FILE__.include?('/work/') && Git.user_email.length > 0)
		list=`nuget list Node.Net`
		if(!list.include?("Node.Net #{VERSION}"))
			puts `nuget push Node.Net.#{VERSION}.nupkg -Source https://api.nuget.org/v3/index.json`
			Git.tag File.dirname(__FILE__), VERSION
		end
	end
end

# version 1.0.314 LOC 5312
# version 1.0.328 LOC 6287
# version 1.0.341 LOC 6463
#
# version 1.0.390 LOC 2281
# version 1.0.420 LOC 2308

# Allow Factories to share Global ManifestResources and ResourceDictionary

# Analyze with SonarQube
# 