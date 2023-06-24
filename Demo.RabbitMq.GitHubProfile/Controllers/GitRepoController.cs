using Demo.RabbitMq.GitHubProfile.Model;
using Demo.RabbitMq.GitHubProfile.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Demo.RabbitMq.GitHubProfile.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GitRepoController : ControllerBase
{
    private readonly IGitRepoRepository _gitRepoRepository;

    public GitRepoController(IGitRepoRepository gitRepoRepository)
    {
        _gitRepoRepository = gitRepoRepository;
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<IEnumerable<GitRepositoryModel>>> GetAllByName([Required] string name, CancellationToken cancellationToken = default)
    { 
        var repositories = await _gitRepoRepository.GetAllByNameAsync(name, cancellationToken);

        if (!repositories.Any())
            return NoContent();

        return Ok(repositories);
    }


}
