using Demo.RabbitMq.GitHubProfile.Model;
using Demo.RabbitMq.GitHubProfile.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Demo.RabbitMq.GitHubProfile.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GitRepoController : ControllerBase
{
    private readonly IGitRepoRepository _gitRepoRepository;
    private readonly IGitRepoRequestsRepository _gitRepoRequestsRepository;

    public GitRepoController(IGitRepoRepository gitRepoRepository, IGitRepoRequestsRepository gitRepoRequestsRepository)
    {
        _gitRepoRepository = gitRepoRepository;
        _gitRepoRequestsRepository = gitRepoRequestsRepository;
    }

    [HttpGet("all/names")]
    public async Task<ActionResult<IEnumerable<string>>> GetNames(CancellationToken cancellationToken = default)
    {
        var names = await _gitRepoRepository.GetRepositoriesNamesAsync(cancellationToken);

        if (!names.Any())
            return NoContent();

        return Ok(names);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<IEnumerable<GitRepositoryModel>>> GetAllByName([Required] string name, CancellationToken cancellationToken = default)
    { 
        var repositories = await _gitRepoRepository.GetAllByNameAsync(name, cancellationToken);

        if (!repositories.Any())
            return NoContent();

        return Ok(repositories);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<GitRepositoryModel>>> AddRequest(GitRepositoryRequestModel model, CancellationToken cancellationToken = default)
    {
        if (!IsOnlyLettersAndNumbersOnModel(model))
            return BadRequest("Modelo inválido, nome deve conter somente letras e números.");

        await _gitRepoRequestsRepository.AddAsync(model, cancellationToken);

        return Created($"api/GitRepo/{model.RequestedName}", model);
    }

    private static bool IsOnlyLettersAndNumbersOnModel(GitRepositoryRequestModel model)
    {
        if (model.RequestedName.All(c => char.IsLetter(c) || char.IsNumber(c)))
            return true;
        return false;
    }
}
