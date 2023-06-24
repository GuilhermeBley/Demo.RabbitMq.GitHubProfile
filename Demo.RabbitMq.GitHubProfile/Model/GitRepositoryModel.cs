namespace Demo.RabbitMq.GitHubProfile.Model;

public class GitRepositoryModel
{
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Topics { get; set; } = new string[0];
    public DateTime CreateAt { get; set; }
}
