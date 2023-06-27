using Demo.RabbitMq.GitHubProfile.Model;

namespace Demo.RabbitMq.GitHubProfile.MessageBus;

public interface IRepoRequestsQueue : IDisposable, IAsyncDisposable
{
    Task<GitRepositoryRequestModel?> TryGetNext();
}

public class RepoRequestsQueue : IRepoRequestsQueue
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GitRepositoryRequestModel?> TryGetNext()
    {
        throw new NotImplementedException();
    }
}