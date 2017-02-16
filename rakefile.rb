VERSION='1.0.312'
require 'dev'

#SERVER_DIR=File.dirname(File.dirname(__FILE__))
#task :setup => [:update]

task :publish  do
	if(__FILE__.include?('/work/') && Git.user_email.length > 0)
		puts `nuget push Node.Net.#{VERSION}.nupkg -Source https://api.nuget.org/v3/index.json`
		Git.tag File.dirname(__FILE__), VERSION
	end
end
