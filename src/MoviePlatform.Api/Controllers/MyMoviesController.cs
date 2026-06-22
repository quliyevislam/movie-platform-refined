using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MoviePlatform.Api.Requests.Movies;
using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Movies;
using MoviePlatform.Application.Movies.Commands.CreateMovie;
using MoviePlatform.Application.Movies.Queries.GetMovieByIdAndUserId;
using MoviePlatform.Application.Movies.Queries.GetMoviesPagedByUserId;

namespace MoviePlatform.Api.Controllers;

[Authorize]
[Route("api/me/movies")]
public sealed class MyMoviesController : ApiController
{
    public MyMoviesController(ISender sender) : base(sender) { }

    [HttpPost]
    public async Task<IActionResult> CreateMovie(
        [FromBody] CreateMovieRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateMovieCommand(
            GetUserId(),
            request.Title,
            request.Description,
            request.Genre,
            request.ReleaseDate);

        Result<Guid> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetMovieById),
            new { movieId = result.Value },
            result.Value);
    }

	[HttpGet("{movieId:guid}")]
    public async Task<IActionResult> GetMovieById(
        Guid movieId,
        CancellationToken cancellationToken)
    {
        var query = new GetMovieByIdAndUserIdQuery(GetUserId(), movieId);

        Result<MovieResponse> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

	[HttpGet]
	public async Task<IActionResult> GetMoviesPaged(
		[FromQuery] int page,
		[FromQuery] int pageSize,
		CancellationToken cancellationToken)
	{
		var query = new GetMoviesPagedByUserIdQuery(
				GetUserId(),
				page,
				pageSize);

		Result<PagedList<MovieResponse>> result = await Sender.Send(query, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}
}
