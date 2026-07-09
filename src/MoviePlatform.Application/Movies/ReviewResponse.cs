namespace MoviePlatform.Application.Movies;

public record ReviewResponse
{
	public Guid ReviewId { get; private set; }
	public Guid UserId { get; private set; }
	public Guid MovieId { get; private set; }
	public int Score { get; private set; }
	public DateTimeOffset CreatedAtUtc { get; private set; }

	public ReviewResponse(
		Guid reviewId,
		Guid userId,
		Guid movieId,
		int score,
		DateTimeOffset createdAtUtc)
	{
		ReviewId = reviewId;
		UserId = userId;
		MovieId = movieId;
		Score = score;
		CreatedAtUtc = createdAtUtc;
	}

	private ReviewResponse() { }
}
