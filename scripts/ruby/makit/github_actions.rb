require "net/http"
require "json"
require "uri"

module Makit
  module GitHubActions
    def self.workflow_status(owner, repo, workflow_id: nil, branch: "main", token: nil)
      # Token should be provided by caller, but fallback to environment variable if not provided
      token ||= ENV["GITHUB_TOKEN"]

      if token.nil? || token.empty?
        raise "GitHub token not found. Set GITHUB_TOKEN environment variable or configure github_token secret."
      end

      base_url = "https://api.github.com/repos/#{owner}/#{repo}"

      if workflow_id
        # Get status for a specific workflow
        url = "#{base_url}/actions/workflows/#{workflow_id}/runs?branch=#{branch}&per_page=1"
      else
        # Get status for all workflows (latest run)
        url = "#{base_url}/actions/runs?branch=#{branch}&per_page=1"
      end

      uri = URI(url)
      http = Net::HTTP.new(uri.host, uri.port)
      http.use_ssl = true

      request = Net::HTTP::Get.new(uri)
      request["Accept"] = "application/vnd.github.v3+json"
      request["Authorization"] = "Bearer #{token}"
      request["User-Agent"] = "Node.Net-Rake-Task"

      response = http.request(request)

      case response.code.to_i
      when 200
        data = JSON.parse(response.body)
        runs = data["workflow_runs"] || []

        if runs.empty?
          return {
                   status: "not_found",
                   message: "No workflow runs found for branch '#{branch}'",
                 }
        end

        latest_run = runs.first
        workflow_name = latest_run["name"] || latest_run.dig("workflow", "name") || "Unknown"

        {
          status: latest_run["status"], # queued, in_progress, completed
          conclusion: latest_run["conclusion"], # success, failure, cancelled, etc.
          workflow_name: workflow_name,
          run_number: latest_run["run_number"],
          html_url: latest_run["html_url"],
          created_at: latest_run["created_at"],
          updated_at: latest_run["updated_at"],
        }
      when 401
        raise "GitHub API authentication failed. Check your token."
      when 404
        raise "Repository #{owner}/#{repo} not found or access denied."
      else
        raise "GitHub API error: #{response.code} - #{response.message}"
      end
    end
  end
end
