module Makit
  module NuGetExt
    def self.publish(package, api_key, source)
      Rake.sh "dotnet nuget push #{package} --skip-duplicate --api-key #{api_key} --source #{source}"
    end
  end
end
