using System.Text;
using System.Threading.Channels;
using Demo.RabbitMq.GitHubProfile.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Demo.RabbitMq.GitHubProfile.MessageBus;

public interface IRepoRequestsQueue : IDisposable
{
    void OnGetData(Action<GitRepositoryRequestModel> onGetData);
    Task AddAsync(GitRepositoryRequestModel model);
}

public class RepoRequestsQueue : IRepoRequestsQueue
{
    private const string QUEUE_NAME = "repo_requests";

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

    public async Task AddAsync(GitRepositoryRequestModel model)
    {
        TryDeclareQueue();

        var jsonText = System.Text.Json.JsonSerializer.Serialize(model);

        var bytes = Encoding.UTF8.GetBytes(jsonText);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: QUEUE_NAME,
            basicProperties: null,
            body: bytes
        );

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
    }

    public void OnGetData(Action<GitRepositoryRequestModel> onGetData)
    {
        TryDeclareQueue();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var jsonText = Encoding.UTF8.GetString(
                ea.Body.ToArray());

            var obj = System.Text.Json.JsonSerializer.Deserialize<GitRepositoryRequestModel>(jsonText);

            if (obj is null)
                return;

            onGetData.Invoke(obj);
        };

        _channel.BasicConsume(
            queue: QUEUE_NAME,
            autoAck: true,
            consumer: consumer
        );
    }

    private void TryDeclareQueue()
        => _channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false, 
            exclusive: false, 
            autoDelete: false
        );
}