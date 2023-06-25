using Demo.RabbitMq.GitHubProfile.Model;
using System.Collections.Concurrent;

namespace Demo.RabbitMq.GitHubProfile.Repositories;

public interface IGitRepoRequestsRepository
{
    Task AddAsync(GitRepositoryRequestModel model, CancellationToken cancellationToken = default);
    Task RemoveAsync(string id, CancellationToken cancellationToken = default);
    Task<GitRepositoryRequestModel?> GetByNameOrDefault(string name, CancellationToken cancellationToken = default);
}

internal class GitRepoRequestsRepository : IGitRepoRequestsRepository
{
    public static readonly ConcurrentDictionary<string, GitRepositoryRequestModel> _poolRequests = new();

    public async Task AddAsync(GitRepositoryRequestModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _poolRequests.GetOrAdd(model.RequestedName, model);

        await Task.CompletedTask;
    }

    public async Task<GitRepositoryRequestModel?> GetByNameOrDefault(string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _poolRequests.TryGetValue(name, out var request);

        return await Task.FromResult(
            request
        );
    }

    public async Task RemoveAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _poolRequests.TryRemove(id, out _);

        await Task.CompletedTask;
    }
}
