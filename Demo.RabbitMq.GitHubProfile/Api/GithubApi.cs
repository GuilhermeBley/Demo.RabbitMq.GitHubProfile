using Demo.RabbitMq.GitHubProfile.Model;
using Microsoft.AspNetCore.Mvc;

namespace Demo.RabbitMq.GitHubProfile.Api;

public interface IGithubApi
{
    Task<IEnumerable<GitRepositoryModel>> GetAllRepositoriesAsync(string name, CancellationToken cancellationToken = default);
}

internal class GithubApi : IGithubApi
{
    private readonly HttpClient _client;

    public GithubApi(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<GitRepositoryModel>> GetAllRepositoriesAsync(string name, CancellationToken cancellationToken = default)
    {
        const string GET = "users/{name}/repos";

        using var response = await _client.GetAsync(GET.Replace("{name}", name));

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return Enumerable.Empty<GitRepositoryModel>();

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<GitRepoJsonModel>>();

        if (result is null)
            return Enumerable.Empty<GitRepositoryModel>();

        return result.Select(r =>
            new GitRepositoryModel {
                CreateAt = r.CreatedAt ?? throw new ArgumentNullException("CreateAt"),
                Description = r.Description ?? string.Empty,
                Name = r.Name ?? string.Empty,
                Topics = r.Topics.ToArray(),
                Url = r.Url ?? string.Empty
            }
        );
    } 
}
