using Demo.RabbitMq.GitHubProfile.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Demo.RabbitMq.GitHubProfile.MessageBus;

public interface IRepoRequestsQueue : IDisposable
{
    Task<GitRepositoryRequestModel?> TryGetNext();
    Task AddAsync(GitRepositoryRequestModel model);
}

public class RepoRequestsQueue : IRepoRequestsQueue
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RepoRequestsQueue(string connectionString)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(connectionString)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task AddAsync(GitRepositoryRequestModel model)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
    }

    public Task<GitRepositoryRequestModel?> TryGetNext()
    {
        throw new NotImplementedException();
    }
}