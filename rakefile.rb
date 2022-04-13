require 'dotkit'

desc 'publish nuget packages'
task :publish => [:publish_local,:publish_to_nuget]

task :integrate => [:publish]

task :default => [:test,:integrate] do
    PROJECT.summary
end