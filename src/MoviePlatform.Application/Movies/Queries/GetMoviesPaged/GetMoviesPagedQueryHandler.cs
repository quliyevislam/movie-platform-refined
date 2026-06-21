using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetMoviesPaged;

internal sealed class GetMoviesPagedQueryHandler
	: IQueryHandler<GetMoviesPagedQuery, PagedList<MovieResponse>>
{
	private readonly IMovieReadRepository _movieReadRepository;

	public GetMoviesPagedQueryHandler(IMovieReadRepository movieReadRepository)
	{
		_movieReadRepository = movieReadRepository;
	}

	public async Task<Result<PagedList<MovieResponse>>> Handle(
		GetMoviesPagedQuery request,
		CancellationToken cancellationToken)
	{
		PagedList<MovieResponse> moviesPagedList = await _movieReadRepository.GetPagedAsync(
			request.Page,
			request.PageSize,
			cancellationToken);

		return Result.Success<PagedList<MovieResponse>>(moviesPagedList);
	}
}
