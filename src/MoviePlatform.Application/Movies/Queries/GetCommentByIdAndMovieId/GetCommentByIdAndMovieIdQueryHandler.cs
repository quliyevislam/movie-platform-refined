using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetCommentByIdAndMovieId;

internal sealed class GetCommentByIdAndMovieIdQueryHandler
	: IQueryHandler<GetCommentByIdAndMovieIdQuery, CommentResponse>
{
	private readonly IMovieReadRepository _movieReadRepository;

	public GetCommentByIdAndMovieIdQueryHandler(IMovieReadRepository movieReadRepository)
	{
		_movieReadRepository = movieReadRepository;
	}

	public async Task<Result<CommentResponse>> Handle(
		GetCommentByIdAndMovieIdQuery request,
		CancellationToken cancellationToken)
	{
		CommentResponse? commentResponse = await _movieReadRepository.GetCommentByIdAndMovieIdAsync(
			request.MovieId,
			request.CommentId,
			cancellationToken);

		if (commentResponse is null)
		{
			return Result.Failure<CommentResponse>(MovieErrors.CommentNotFound);
		}

		return Result.Success<CommentResponse>(commentResponse);
	}
}
