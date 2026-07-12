using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetReviewByIdAndMovieId;

internal sealed class GetReviewByIdAndMovieIdQueryHandler
	: IQueryHandler<GetReviewByIdAndMovieIdQuery, ReviewResponse>
{
	private readonly IMovieReadRepository _movieReadRepository;

	public GetReviewByIdAndMovieIdQueryHandler(IMovieReadRepository movieReadRepository)
	{
		_movieReadRepository = movieReadRepository;
	}

	public async Task<Result<ReviewResponse>> Handle(
		GetReviewByIdAndMovieIdQuery request,
		CancellationToken cancellationToken)
	{
		ReviewResponse? reviewResponse = await _movieReadRepository.GetReviewByIdAndMovieIdAsync(
			request.MovieId,
			request.ReviewId,
			cancellationToken);

		if (reviewResponse is null)
		{
			return Result.Failure<ReviewResponse>(MovieErrors.ReviewNotFound);
		}

		return Result.Success<ReviewResponse>(reviewResponse);
	}
}
