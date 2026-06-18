using MoviePlatform.Domain.Movies.ValueObjects;

namespace MoviePlatform.Domain.Movies;

public interface IMovieWriteRepository
{
	Task<Movie?> GetByIdAsync(MovieId movieId, CancellationToken cancellationToken = default);
	Task<Movie?> GetByIdWithCommentsAsync(MovieId movieId, CancellationToken cancellationToken = default);
	void Add(Movie movie);
	void Remove(Movie movie);
}
