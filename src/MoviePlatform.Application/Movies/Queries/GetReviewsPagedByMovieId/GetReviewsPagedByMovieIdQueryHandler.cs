using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetReviewsPagedByMovieId;

internal sealed class GetReviewsPagedByMovieIdQueryHandler
	: IQueryHandler<GetReviewsPagedByMovieIdQuery, PagedList<ReviewResponse>>
{
	private readonly IMovieReadRepository _movieReadRepository;

	public GetReviewsPagedByMovieIdQueryHandler(IMovieReadRepository movieReadRepository)
	{
		_movieReadRepository = movieReadRepository;
	}

	public async Task<Result<PagedList<ReviewResponse>>> Handle(
		GetReviewsPagedByMovieIdQuery request,
		CancellationToken cancellationToken)
	{
		PagedList<ReviewResponse> reviewsPagedList = await _movieReadRepository.GetReviewsPagedByMovieIdAsync(
			request.MovieId,
			request.Page,
			request.PageSize,
			cancellationToken);

		return Result.Success<PagedList<ReviewResponse>>(reviewsPagedList);
	}
}
