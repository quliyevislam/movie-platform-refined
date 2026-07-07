using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MoviePlatform.Api.Requests.Movies;
using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Application.Movies;
using MoviePlatform.Application.Movies.Commands.CreateMovie;
using MoviePlatform.Application.Movies.Commands.UpdateMovie;
using MoviePlatform.Application.Movies.Commands.DeleteMovie;
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
            nameof(GetMovieByIdAndUserId),
            new { movieId = result.Value },
            result.Value);
    }

	[HttpPut("{movieId:guid}")]
	public async Task<IActionResult> UpdateMovie(
		Guid movieId,
		UpdateMovieRequest request,
		CancellationToken cancellationToken)
	{
		var command = new UpdateMovieCommand(
			GetUserId(),
			movieId,
			request.Title,
			request.Description,
			request.Genre,
			request.ReleaseDate);

		Result<Guid> result = await Sender.Send(command, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}

	[HttpDelete("{movieId:guid}")]
	public async Task<IActionResult> DeleteMovie(
		Guid movieId,
		CancellationToken cancellationToken)
	{
		var command = new DeleteMovieCommand(GetUserId(), movieId);

		Result result = await Sender.Send(command, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok();
	}

	[HttpGet("{movieId:guid}")]
    public async Task<IActionResult> GetMovieByIdAndUserId(
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
	public async Task<IActionResult> GetMoviesPagedByUserId(
		CancellationToken cancellationToken = default,
		[FromQuery] int page = MovieConstants.DefaultPage,
		[FromQuery] int pageSize = MovieConstants.DefaultPageSize)
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
