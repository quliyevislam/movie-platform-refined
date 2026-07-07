namespace MoviePlatform.Api.Requests.Movies;

public record UpdateMovieRequest(
	string Title,
	string? Description,
	string? Genre,
	DateOnly ReleaseDate);
