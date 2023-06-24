using Demo.RabbitMq.GitHubProfile.Model;
using System.Collections.Concurrent;

namespace Demo.RabbitMq.GitHubProfile.Repositories;

internal interface IGitRepoRepository
{
    Task AddAsync(string name, GitRepositoryModel[] gitRepositoryModels, CancellationToken cancellationToken = default);
    Task RemoveByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<GitRepositoryModel>> GetAllByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetRepositoriesNamesAsync(CancellationToken cancellationToken = default);
}

internal class GitRepoRepository : IGitRepoRepository
{
    private static readonly ConcurrentDictionary<string, IEnumerable<GitRepositoryModel>> _poolGitRepo = new();

    public async Task AddAsync(string name, GitRepositoryModel[] gitRepositoryModels, CancellationToken cancellationToken = default)
        => await Task.FromResult( 
            _poolGitRepo.AddOrUpdate(name, gitRepositoryModels, (name, current) => gitRepositoryModels));

    public async Task<IEnumerable<GitRepositoryModel>> GetAllByNameAsync(string name, CancellationToken cancellationToken = default)
        => await Task.FromResult(
            _poolGitRepo.GetValueOrDefault(name) ?? Enumerable.Empty<GitRepositoryModel>());

    public async Task<IEnumerable<string>> GetRepositoriesNamesAsync(CancellationToken cancellationToken = default)
        => await Task.FromResult(
            _poolGitRepo.Keys    
        );

    public async Task RemoveByNameAsync(string name, CancellationToken cancellationToken = default)
        => await Task.FromResult(
            _poolGitRepo.TryRemove(name, out IEnumerable<GitRepositoryModel>? _)    
        );
}
