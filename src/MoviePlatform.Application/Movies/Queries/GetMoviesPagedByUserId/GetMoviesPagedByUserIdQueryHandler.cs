using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetMoviesPagedByUserId;

internal sealed class GetMoviesPagedByUserIdQueryHandler
	: IQueryHandler<GetMoviesPagedByUserIdQuery, PagedList<MovieResponse>>
{
	private readonly IMovieReadRepository _movieReadRepository;

	public GetMoviesPagedByUserIdQueryHandler(IMovieReadRepository movieReadRepository)
	{
		_movieReadRepository = movieReadRepository;
	}

	public async Task<Result<PagedList<MovieResponse>>> Handle(
		GetMoviesPagedByUserIdQuery request,
		CancellationToken cancellationToken)
	{
		PagedList<MovieResponse> moviesPagedList = await _movieReadRepository.GetPagedByUserIdAsync(
			request.UserId,
			request.Page,
			request.PageSize,
			cancellationToken);

		return Result.Success<PagedList<MovieResponse>>(moviesPagedList);
	}
}
