namespace MoviePlatform.Application.Movies;

public record MovieResponse(
	Guid MovieId,
	Guid UserId,
	string Title,
	string? Description,
	string Genre,
	DateOnly ReleaseDate,
	double AverageScore,
	int ReviewCount,
	DateTimeOffset CreatedAtUtc);
