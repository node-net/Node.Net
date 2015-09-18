VERSION='0.0.220'
require 'dev'

CLOBBER.include('lib')
CLOBBER.include('*.{zip,nupkg}')

task :publish => [:build,:test,:commit] do
	if(__FILE__.include?('/wrk/'))
		Git.tag File.dirname(__FILE__), VERSION
	end

	#Text.replace_in_file('Node.Net.nuspec',/<version>(\d+.\d+.\d+)<\/version>/,"<version>#{VERSION}</version>")
	puts `nuget push Node.Net.#{VERSION}.nupkg`
end


task :increment_version do
	major=IO.read(__FILE__).scan( /VERSION='([\d])+.[\d]+.[\d]+'/).last.first
	minor=IO.read(__FILE__).scan( /VERSION='[\d]+.([\d]+).[\d]+'/).last.first
	build=IO.read(__FILE__).scan( /VERSION='[\d]+.[\d]+.([\d]+)'/).last.first
	next_build=(build.to_i+1).to_s
	if(Git.has_changes?)
		puts "updating VERSION from #{major}.#{minor}.#{build} to #{major}.#{minor}.#{next_build}"
		Text.replace_in_file('rakefile.rb',"VERSION='#{major}.#{minor}.#{build}'","VERSION='#{major}.#{minor}.#{next_build}'")
		#VERSION="#{major}.#{minor}.#{next_build}"
	end
end
