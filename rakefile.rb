VERSION='0.0.258'
require 'dev'

CLOBBER.include('lib')
CLOBBER.include('*.{zip,nupkg}')

task :publish => [:build,:test,:commit] do
	if(__FILE__.include?('/wrk/') && Git.user_email.length > 0)
		Git.tag File.dirname(__FILE__), VERSION
		puts `nuget push Node.Net.#{VERSION}.nupkg`
	end
end

