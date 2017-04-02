VERSION='1.0.374'
require 'dev'

task :publish  do
	if(__FILE__.include?('/work/') && Git.user_email.length > 0)
		puts `nuget push Node.Net.#{VERSION}.nupkg -Source https://api.nuget.org/v3/index.json`
		Git.tag File.dirname(__FILE__), VERSION
	end
end

# version 1.0.314 LOC 5312
# version 1.0.328 LOC 6287
# version 1.0.341 LOC 6463