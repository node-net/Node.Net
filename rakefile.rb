
#
# https://gitlab.com/lou-parslow/Node.Net.git
#
VERSION='1.0.310'
require 'dev'

#PROJECTS={
#	'Node.Net.Collections' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Collections' },
#	'Node.Net.Controls' => {:url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Controls' },
#	'Node.Net.Data' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Data' },
#	'Node.Net.Diagnostics' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Diagnostics' },
#	'Node.Net.Extensions' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Extensions' },
#	'Node.Net.Factories' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Factories' },
#	'Node.Net.Measurement' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Measurement' },
#	'Node.Net.Readers' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Readers' },
#	'Node.Net.Repositories' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Repositories' },
#	'Node.Net.Writers' => { :url => 'https://lou-parslow.visualstudio.com/DefaultCollection/_git/Node.Net.Writers' },
#}
SERVER_DIR=File.dirname(File.dirname(__FILE__))
task :setup => [:update]
#task :update => [:update_projects, :update_source]
#task :update_projects do
#	puts ":update_projects"
#	PROJECTS.each{|name,project|
#	  puts name
#	  work="#{SERVER_DIR}/#{name}"
#	  url="#{project[:url]}"
#	  
#	  puts "url #{url}"
#	  if(Dir.exists?(work))
#		puts Command.execute("git pull",work).summary(true)
#	  else
#		puts Command.execute("git clone #{url} #{work}").summary
#	  end
#	}
#end

#task :update_source do
#	puts ":update_source"
#
#	glob_pattern='**/*{cs,xaml,txt}'
#	exclude_patterns=['bin','obj','AssemblyInfo.cs']
#
#	PROJECTS.each{|name,project|
#		puts name
#		lib = name.gsub('Node.Net.','')
#		# Source file
#		src_dir="#{SERVER_DIR}/#{name}/#{name}"
#		target_dir="#{SERVER_DIR}/Node.Net/Node.Net/#{lib}"
#		puts "copying source files from #{src_dir} to #{target_dir}"
#		glob_pattern='**/*{cs,xaml,txt}'
#		Dir.copy_files src_dir,glob_pattern,exclude_patterns,target_dir
#
#		# Test source files
#		src_dir="#{SERVER_DIR}/#{name}/#{name}.Test"
#		glob_pattern='**/*{cs,xaml,json}'
#		target_dir="#{SERVER_DIR}/Node.Net/Node.Net.Test/#{lib}"
#		puts "copying source files from #{src_dir} to #{target_dir}"
#		Dir.copy_files src_dir,glob_pattern,exclude_patterns,target_dir
#	}
#end

#task :publish_to_github do
#	
#	url='http://github.com/node-net/Node.Net.git'
#	dir="#{Environment.dev_root}/work/github/node-net/Node.Net"
#
#	puts `git clone #{url} #{dir}` if(!Dir.exists?(dir))
#	Dir.chdir(dir) do
#		puts `git pull`
#	end
#	FileUtils.rm_rf("#{dir}/Node.Net")
#
#	files = FileList.new('LICENSE.txt','README.md','Node.Net.sln','Node.Net/**/*')
#	files.each{|f|
#		dest="#{dir}/#{f}"
#		if(File.file?(f))
#		  parent_dir = File.dirname(dest)
#		  if(parent_dir.length > 0 && !Dir.exists?(parent_dir))
#			FileUtils.mkdir_p(parent_dir)
#		  end
#		  #puts "copying #{f} to #{dest}"
#		  FileUtils.cp(f,dest)
#		end
#	}
#
#	Dir.chdir(dir) do
#		puts "(#{dir})"
#		puts `git add --all`
#		puts `git tag #{VERSION} -m#{VERSION}`
#		puts `git commit -m'#{VERSION}'`
#		puts `git push --tags`
#		puts `git push`
#	end
#
#end

task :publish  do
	if(__FILE__.include?('/work/') && Git.user_email.length > 0)
		puts `nuget push Node.Net.#{VERSION}.nupkg -Source https://api.nuget.org/v3/index.json`
		Git.tag File.dirname(__FILE__), VERSION
	end
end
