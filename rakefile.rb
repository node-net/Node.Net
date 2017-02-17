VERSION='1.0.317'
require 'dev'

task :publish  do
	if(__FILE__.include?('/work/') && Git.user_email.length > 0)
		puts `nuget push Node.Net.#{VERSION}.nupkg -Source https://api.nuget.org/v3/index.json`
		Git.tag File.dirname(__FILE__), VERSION
	end
end

# version 1.0.314 LOC 5312