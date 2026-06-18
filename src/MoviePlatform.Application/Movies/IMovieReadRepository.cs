using MoviePlatform.Domain.Common;

namespace MoviePlatform.Application.Movies;

public interface IMovieReadRepository
{
	Task<PagedList<MovieResponse>> GetByUserIdAsync(
		Guid userId,
		int page,
		int pageSize,
		CancellationToken cancellationToken = default);
}
