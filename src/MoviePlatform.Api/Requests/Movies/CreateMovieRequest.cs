namespace MoviePlatform.Api.Requests.Movies;

public record CreateMovieRequest(
	string Title,
	string? Description,
	string? Genre,
	DateOnly ReleaseDate);
