namespace MoviePlatform.Application.Movies;

public record MovieResponse
{
	public Guid MovieId { get; private set; }
	public Guid UserId { get; private set; }
	public string Title { get; private set; } = default!;
	public string? Description { get; private set; }
	public string? Genre { get; private set; }
	public DateOnly ReleaseDate { get; private set; }
	public double AverageScore { get; private set; }
	public int ReviewCount { get; private set; }
	public DateTimeOffset CreatedAtUt { get; private set; }

	public MovieResponse(
		Guid movieId,
		Guid userId,
		string title,
		string? description,
		string genre,
		DateOnly releaseDate,
		double averageScore,
		int reviewCount,
		DateTimeOffset createdAtUt)
	{
		MovieId = movieId;
		UserId = userId;
		Title = title;
		Description = description;
		Genre = genre;
		ReleaseDate = releaseDate;
		AverageScore = averageScore;
		ReviewCount = reviewCount;
		CreatedAtUt = createdAtUt;
	}

	private MovieResponse() {}
}
