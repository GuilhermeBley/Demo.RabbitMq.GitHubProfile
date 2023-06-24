using Demo.RabbitMq.GitHubProfile.Model;

namespace Demo.RabbitMq.GitHubProfile.Api;

internal interface IGithubApi
{
    Task<IEnumerable<GitRepositoryModel>> GetAllRepositoriesAsync(string name, CancellationToken cancellationToken = default);
}

internal class GithubApi : IGithubApi
{
    public Task<IEnumerable<GitRepositoryModel>> GetAllRepositoriesAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
