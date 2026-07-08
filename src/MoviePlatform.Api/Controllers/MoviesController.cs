using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Application.Movies.Queries.GetMovieById;
using MoviePlatform.Application.Movies.Queries.GetMoviesPaged;
using MoviePlatform.Application.Movies;
namespace MoviePlatform.Api.Controllers;

[Route("/api/movies")]
public sealed class MoviesController : ApiController
{
	public MoviesController(ISender sender) : base(sender) { }

	[HttpGet]
	public async Task<IActionResult> GetMoviesPaged(
		CancellationToken cancellationToken = default,
		[FromQuery] int page = MovieConstants.DefaultPage,
		[FromQuery] int pageSize = MovieConstants.DefaultPageSize)
	{
		var query = new GetMoviesPagedQuery(page, pageSize);

		Result<PagedList<MovieResponse>> result = await Sender.Send(query, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}

	[HttpGet("{movieId:guid}")]
	public async Task<IActionResult> GetMovieById(
		Guid movieId,
		CancellationToken cancellationToken)
	{
		var query = new GetMovieByIdQuery(movieId);

		Result<MovieResponse> result = await Sender.Send(query, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}
}
