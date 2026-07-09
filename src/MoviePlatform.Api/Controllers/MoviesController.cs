using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Application.Movies.Queries.GetMovieById;
using MoviePlatform.Application.Movies.Queries.GetMoviesPaged;
using MoviePlatform.Application.Movies.Queries.GetReviewsPagedByMovieId;
using MoviePlatform.Application.Movies.Commands.SubmitReview;
using MoviePlatform.Application.Movies;
using MoviePlatform.Api.Requests.Movies;

namespace MoviePlatform.Api.Controllers;

[Route("api/movies")]
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

	[Authorize]
	[HttpPost("{movieId:guid}/reviews")]
	public async Task<IActionResult> SubmitReview(
		Guid movieId,
		SubmitReviewRequest request,
		CancellationToken cancellationToken)
	{
		var command = new SubmitReviewCommand(
			GetUserId(),
			movieId,
			request.Score);

		Result<Guid> result = await Sender.Send(command, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}


		// return CreatedAtAction(
  //           nameof(GetMovieByIdAndUserId),
  //           new { movieId = result.Value },
  //           result.Value);

		return Ok(result.Value);
	}

	[HttpGet("{movieId:guid}/reviews")]
	public async Task<IActionResult> GetReviewsPagedByMovieId(
		Guid movieId,
		[FromQuery] int page = MovieConstants.DefaultPage,
		[FromQuery] int pageSize = MovieConstants.DefaultPageSize,
		CancellationToken cancellationToken = default)
	{
		var query = new GetReviewsPagedByMovieIdQuery(movieId, page, pageSize);

		Result<PagedList<ReviewResponse>> result = await Sender.Send(query, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}
}
