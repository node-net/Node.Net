require 'dev'

VERSION='0.0.217'

CLOBBER.include('lib')
CLOBBER.include('*.{zip,nupkg}')

task :increment_version do
	major=IO.read(__FILE__).scan( /VERSION='([\d])+.[\d]+.[\d]+'/).last.first
	minor=IO.read(__FILE__).scan( /VERSION='[\d]+.([\d]+).[\d]+'/).last.first
	build=IO.read(__FILE__).scan( /VERSION='[\d]+.[\d]+.([\d]+)'/).last.first
	next_build=(build.to_i+1).to_s
	if(Git.has_changes?)
		puts "updating VERSION from #{major}.#{minor}.#{build} to #{major}.#{minor}.#{next_build}"
		Text.replace_in_file('rakefile.rb',"VERSION='#{major}.#{minor}.#{build}'","VERSION='#{major}.#{minor}.#{next_build}'")
		VERSION="#{major}.#{minor}.#{next_build}"
	end
end



vs_version=MSBuild.get_vs_version('Node.Net.Test.sln')
#COMMANDS[:build] = ["\"#{MSBuild.get_version(vs_version)}\" Node.Net.Test.sln /nologo /p:Configuration=Release /p:Platform=\"Any CPU\"",
#	                "\"#{MSBuild.get_version(vs_version)}\" Node.Net.Test.sln /nologo /p:Configuration=Debug /p:Platform=\"Any CPU\"",
#					"\"#{MSBuild.get_version(vs_version)}\" Node.Net.Libs.sln /nologo /p:Configuration=Release /p:Platform=\"Any CPU\"",
#				    "\"#{MSBuild.get_version(vs_version)}\" Node.Net.Libs.sln /nologo /p:Configuration=Debug /p:Platform=\"Any CPU\""]		      			
#COMMANDS[:test] = ["\"#{Test.nunit_console}\" \"#{Rake.application.original_dir}/bin/Net4.6/Release/Node.Net.Test.dll\" /xml:\"bin/Net4.6/Release/Node.Net.Test.dll.TestResults.xml\""]
task :build => [:increment_version] 
task :publish => [:build,:test,:commit] do
	puts "publish #{__FILE__}}"
	if(__FILE__.include?('/wrk/'))
		Git.tag File.dirname(__FILE__), VERSION
		source=FileList.new('Node.Net.sln','Node.Net/**/*.{csproj,snk,cs,json,xaml}',
							'bin/Net4.6/Release/Node.Net.dll','bin/Net4.6/Release/Node.Net.pdb',
							'bin/Net4.6/Debug/Node.Net.dll','bin/Net4.6/Debug/Node.Net.pdb')
		Git.publish "https://github.com/lou-parslow/Node.Net.git" ,File.dirname(__FILE__), source, VERSION
	end

	Text.replace_in_file('Node.Net.nuspec',/<version>(\d+.\d+.\d+)<\/version>/,"<version>#{VERSION}</version>")

	FileUtils.mkdir('lib') if !File.exists?('lib')
	FileUtils.mkdir('lib/Net4.6') if !File.exists?('lib/Net4.6')
	FileUtils.cp('bin/Net4.6/Release/Node.Net.dll','lib/Net4.6/Node.Net.dll')
	puts `nuget pack Node.Net.nuspec`
	puts `nuget push Node.Net.#{VERSION}.nupkg`
end

task :default => [:pull]
