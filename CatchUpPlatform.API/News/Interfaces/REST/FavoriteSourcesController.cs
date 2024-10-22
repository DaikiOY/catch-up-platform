using System.Net.Mime;
using CatchUpPlatform.API.News.Domain.Model.Queries;
using CatchUpPlatform.API.News.Domain.Services;
using CatchUpPlatform.API.News.Interfaces.REST.Resources;
using CatchUpPlatform.API.News.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CatchUpPlatform.API.News.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Favorite Source")]
public class FavoriteSourcesController(
    IFavoriteSourceCommandService favoriteSourceCommandService,
    IFavoriteSourceQueryService favoriteSourceQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a favorite source",
        Description = "Create a favorite source with a given News API Key and Source ID",
        OperationId = "CreateFavoriteSource"
        )]
    [SwaggerResponse(201, "The favorite source was created", typeof(FavoriteSourceResource))]
    public async Task<ActionResult> CreateFavoriteSource([FromBody] CreateFavoriteSourceResource resource)
    {
        var createFavoriteSourceCommand = CreateFavoriteSourceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await favoriteSourceCommandService.Handle(createFavoriteSourceCommand);

        if (result is null) return BadRequest();

        return CreatedAtAction(nameof(GetFavoriteSourceById), new { id = result.Id }, FavoriteSourceResourceFromEntityAssembler.ToResourceFromEntity(result));

    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets a favorite source according to parameters",
        Description = "Gets a favorite source for given parameters",
        OperationId = "GetFavoriteSourceByNewsApiKeyAndSourceId"
        )]
    [SwaggerResponse(200, "Result(s) was/were found", typeof(FavoriteSourceResource))]
    public async Task<ActionResult> GetFavoriteSourceByNewsApiKeyAndSourceId([FromQuery] string newsApiKey, [FromQuery] string sourceId)
    {
        var getFavoriteSourceByNewsApiKeyAndSourceIdQuery = new GetFavoriteSourceByNewsApiKeyAndSourceIdQuery(newsApiKey, sourceId);
        var result = await favoriteSourceQueryService.Handle(getFavoriteSourceByNewsApiKeyAndSourceIdQuery);
        if (result is null) return BadRequest();
        var resource = FavoriteSourceResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Gets a favorite source by id",
        Description = "Gets a favorite source for given favorite source identifier",
        OperationId = "GetFavoriteSourceById"
        )]
    [SwaggerResponse(200, "The favorite source was found", typeof(FavoriteSourceResource))]
    public async Task<ActionResult> GetFavoriteSourceById(int id)
    {
        var getFavoriteSourceByIdQuery = new GetFavoriteSourceByIdQuery(id);
        var result = await favoriteSourceQueryService.Handle(getFavoriteSourceByIdQuery);
        if (result is null) return BadRequest();
        var resource = FavoriteSourceResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
}

