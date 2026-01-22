require "net/http"
require "json"
require "uri"

module Makit
  module GitHubActions
    # Gets the current git branch, defaulting to "main" if not available
    #
    # @return [String] Current branch name
    def self.get_current_branch
      branch = `git rev-parse --abbrev-ref HEAD`.strip
      branch.empty? ? "main" : branch
    end

    # Gets GitHub token from secrets or environment variable
    #
    # @return [String, nil] GitHub token if available
    def self.get_token
      if defined?(Makit::Secrets) && Makit::Secrets.has_key?("github_token")
        Makit::Secrets.get("github_token")
      else
        ENV["GITHUB_TOKEN"]
      end
    end

    # Formats and displays workflow status information
    #
    # @param result [Hash] Workflow status result from workflow_status
    # @param owner [String] Repository owner (for display)
    # @param repo [String] Repository name (for display)
    # @param branch [String] Branch name (for display)
    def self.display_status(result, owner, repo, branch)
      if result[:status] == "not_found"
        puts "⚠️  #{result[:message]}"
      else
        status_emoji = case result[:conclusion]
          when "success"
            "✅"
          when "failure"
            "❌"
          when "cancelled"
            "🚫"
          when nil
            result[:status] == "completed" ? "⏸️" : "🔄"
          else
            "❓"
          end

        puts "\n#{status_emoji} Workflow Status: #{result[:workflow_name] || "Unknown"}"
        puts "   Status: #{result[:status]}"
        puts "   Conclusion: #{result[:conclusion] || "N/A"}" if result[:conclusion]
        puts "   Run Number: ##{result[:run_number]}" if result[:run_number]
        puts "   Created: #{result[:created_at]}" if result[:created_at]
        puts "   Updated: #{result[:updated_at]}" if result[:updated_at]
        puts "   URL: #{result[:html_url]}" if result[:html_url]
      end
    end

    # Queries and displays GitHub Actions workflow status
    # Handles all logic including branch detection, token retrieval, and output formatting
    #
    # @param branch [String, nil] Branch name to query (default: auto-detect from git)
    # @param workflow_id [String, nil] Specific workflow ID to query (optional)
    # @param token [String, nil] GitHub Personal Access Token (optional, will try secrets/env if not provided)
    # @raise [RuntimeError] If authentication fails or API error occurs
    def self.query_and_display_status(branch: nil, workflow_id: nil, token: nil)
      branch ||= get_current_branch
      owner, repo = get_repo_from_git

      puts "Querying GitHub Actions status for #{owner}/#{repo} (branch: #{branch})..."

      token ||= get_token
      if token.nil? || token.empty?
        puts "github_token SECRET not available"
      end

      result = workflow_status(branch: branch, workflow_id: workflow_id, token: token)
      display_status(result, owner, repo, branch)
    rescue => e
      puts "❌ Error: #{e.message}"
      raise
    end
    # Extracts GitHub repository owner and name from a URL or git remote URL
    #
    # @param url [String] GitHub repository URL (https://github.com/owner/repo or git@github.com:owner/repo)
    # @return [Array<String>] Array containing [owner, repo_name]
    # @raise [ArgumentError] If URL cannot be parsed
    #
    # @example HTTPS URL format
    #   parse_repo_url("https://github.com/node-net/Node.Net.git")
    #   # => ["node-net", "Node.Net"]
    #
    # @example SSH URL format
    #   parse_repo_url("git@github.com:node-net/Node.Net.git")
    #   # => ["node-net", "Node.Net"]
    #
    # @example URL without .git extension
    #   parse_repo_url("https://github.com/microsoft/dotnet")
    #   # => ["microsoft", "dotnet"]
    def self.parse_repo_url(url)
      match = url.match(%r{github\.com[:/]([^/]+)/([^/]+)(?:\.git)?$})
      if match
        owner = match[1]
        repo = match[2].gsub(/\.git$/, "")
        return [owner, repo]
      else
        raise ArgumentError, "Could not parse GitHub repository from URL: #{url}"
      end
    end

    # Gets GitHub repository info from git remote origin
    #
    # @return [Array<String>] Array containing [owner, repo_name]
    # @raise [RuntimeError] If git remote URL cannot be determined or parsed
    def self.get_repo_from_git
      remote_url = `git config --get remote.origin.url`.strip
      if remote_url.empty?
        raise "Could not determine git remote URL. Make sure you're in a git repository with a remote configured."
      end
      parse_repo_url(remote_url)
    end

    # Queries GitHub Actions API for workflow run status
    #
    # @param owner [String, nil] Repository owner (required if repo_url is nil)
    # @param repo [String, nil] Repository name (required if repo_url is nil)
    # @param repo_url [String, nil] GitHub repository URL or git remote URL (alternative to owner/repo)
    # @param workflow_id [String, nil] Specific workflow ID to query (optional)
    # @param branch [String] Branch name to query (default: "main")
    # @param token [String, nil] GitHub Personal Access Token (optional, will use GITHUB_TOKEN env var if not provided)
    # @return [Hash] Workflow status information
    # @raise [RuntimeError] If authentication fails or API error occurs
    def self.workflow_status(owner: nil, repo: nil, repo_url: nil, workflow_id: nil, branch: "main", token: nil)
      # Determine owner and repo from URL if provided
      if repo_url
        owner, repo = parse_repo_url(repo_url)
      elsif owner.nil? || repo.nil?
        # Try to get from git if not provided
        owner, repo = get_repo_from_git
      end
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
