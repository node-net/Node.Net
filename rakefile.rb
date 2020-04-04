#NAME="Node.Net"
require 'dotkit'

desc 'publish nuget packages'
task :publish => [:publish_local,:publish_to_nuget,:delete_from_nuget]

task :integrate => [:publish]

task :default => [:integrate] do
    PROJECT.summary
end