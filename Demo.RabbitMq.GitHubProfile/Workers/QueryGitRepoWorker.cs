using Demo.RabbitMq.GitHubProfile.Repositories;

namespace Demo.RabbitMq.GitHubProfile.Workers;

public class QueryGitRepoWorker : BackgroundService
{
    private IGitRepoRequestsRepository _gitRepoRequestsRepository;
    private readonly ILogger<QueryGitRepoWorker> _log;

    public QueryGitRepoWorker(ILogger<QueryGitRepoWorker> log)
    {
        _gitRepoRequestsRepository = new GitRepoRequestsRepository();
        _log = log;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _log.LogInformation($"QueryGitRepoWorker run at: '{DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss")}' (UTC)");

            await Task.Delay(1000, stoppingToken);
        }
    }
}
