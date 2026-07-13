using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Application.Movies.Queries.GetMovieById;
using MoviePlatform.Application.Movies.Queries.GetMoviesPaged;
using MoviePlatform.Application.Movies.Commands.SubmitReview;
using MoviePlatform.Application.Movies.Queries.GetReviewsPagedByMovieId;
using MoviePlatform.Application.Movies.Queries.GetReviewByIdAndMovieId;
using MoviePlatform.Application.Movies.Commands.AddComment;
using MoviePlatform.Application.Movies.Commands.UpdateComment;
using MoviePlatform.Application.Movies.Commands.DeleteComment;
using MoviePlatform.Application.Movies.Queries.GetCommentByIdAndMovieId;
using MoviePlatform.Application.Movies.Queries.GetCommentsPagedByMovieId;
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

		return CreatedAtAction(
            nameof(GetReviewByIdAndMovieId),
            new { movieId = movieId, reviewId = result.Value },
            result.Value);
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

	[HttpGet("{movieId:guid}/reviews/{reviewId:guid}")]
	public async Task<IActionResult> GetReviewByIdAndMovieId(
		Guid movieId,
		Guid reviewId,
		CancellationToken cancellationToken = default)
	{
		var query = new GetReviewByIdAndMovieIdQuery(movieId, reviewId);

		Result<ReviewResponse> result = await Sender.Send(query, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}
	
	[Authorize]
	[HttpPost("{movieId:guid}/comments")]
	public async Task<IActionResult> AddComment(
		Guid movieId,
		AddCommentRequest request,
		CancellationToken cancellationToken = default)
	{
		var command = new AddCommentCommand(GetUserId(), movieId,  request.Content);

		Result<Guid> result = await Sender.Send(command, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return CreatedAtAction(
            nameof(GetCommentByIdAndMovieId),
            new { movieId = movieId, commentId = result.Value },
            result.Value);
	}

	[Authorize]
	[HttpPut("{movieId:guid}/comments/{commentId:guid}")]
	public async Task<IActionResult> UpdateComment(
		Guid movieId,
		Guid commentId,
		UpdateCommentRequest request,
		CancellationToken cancellationToken = default)
	{
		var command = new UpdateCommentCommand(
			GetUserId(),
			movieId,
			commentId,
			request.Content);

		Result<Guid> result = await Sender.Send(command, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}

	[Authorize]
	[HttpDelete("{movieId:guid}/comments/{commentId:guid}")]
	public async Task<IActionResult> DeleteComment(
		Guid movieId,
		Guid commentId,
		CancellationToken cancellationToken = default)
	{
		var command = new DeleteCommentCommand(
			GetUserId(),
			movieId,
			commentId);

		Result result = await Sender.Send(command, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok();
	}
	
	[HttpGet("{movieId:guid}/comments")]
	public async Task<IActionResult> GetCommentsPagedByMovieIdQuery(
		Guid movieId,
		[FromQuery] int page = MovieConstants.DefaultPage,
		[FromQuery] int pageSize = MovieConstants.DefaultPageSize,
		CancellationToken cancellationToken = default)
	{
		var query = new GetCommentsPagedByMovieIdQuery(movieId, page, pageSize);

		Result<PagedList<CommentResponse>> result = await Sender.Send(query, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}

	[HttpGet("{movieId:guid}/comments/{commentId:guid}")]
	public async Task<IActionResult> GetCommentByIdAndMovieId(
		Guid movieId,
		Guid commentId,
		CancellationToken cancellationToken = default)
	{
		var query = new GetCommentByIdAndMovieIdQuery(movieId, commentId);

		Result<CommentResponse> result = await Sender.Send(query, cancellationToken);

		if (result.IsFailure)
		{
			return HandleFailure(result);
		}

		return Ok(result.Value);
	}
}
