using Demo.RabbitMq.GitHubProfile.Api;
using Demo.RabbitMq.GitHubProfile.Repositories;
using Demo.RabbitMq.GitHubProfile.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<QueryGitRepoWorker>();

builder.Services.AddHttpClient("git", (client) =>
{
    client.BaseAddress = new Uri("https://api.github.com");
});

builder.Services.AddSingleton<IGithubApi, GithubApi>(
    (provider) => new GithubApi(provider.GetRequiredService<IHttpClientFactory>().CreateClient("git")));
builder.Services.AddScoped<IGitRepoRepository, GitRepoRepository>();
builder.Services.AddScoped<IGitRepoRequestsRepository, GitRepoRequestsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
