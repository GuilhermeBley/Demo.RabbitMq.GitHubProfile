using Demo.RabbitMq.GitHubProfile.Api;
using Demo.RabbitMq.GitHubProfile.Model;
using Demo.RabbitMq.GitHubProfile.Repositories;

namespace Demo.RabbitMq.GitHubProfile.Workers;

public class QueryGitRepoWorker : BackgroundService
{
    private IGitRepoRequestsRepository _gitRepoRequestsRepository;
    private readonly IGitApi _gitApi;
    private readonly GitRepoRepository _gitRepoRepository;
    private readonly ILogger<QueryGitRepoWorker> _log;

    public QueryGitRepoWorker(ILogger<QueryGitRepoWorker> log, IGitApi gitApi)
    {
        _gitRepoRequestsRepository = new GitRepoRequestsRepository();
        _gitApi = gitApi;
        _gitRepoRepository = new GitRepoRepository();
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

    private async Task TryGetAndSetGitRepositoryByRequest(GitRepositoryRequestModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var repositoriesFound = await _gitApi.GetAllRepositoriesAsync(model.RequestedName, cancellationToken: cancellationToken);

        if (repositoriesFound is null)
            return;

        await _gitRepoRepository.AddAsync(
            model.RequestedName,
            repositoriesFound.ToArray(),
            cancellationToken: cancellationToken
        );
    }
}
