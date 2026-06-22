using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Movies.Entities;

public sealed class Review : BaseEntity<ReviewId>
{
	public UserId UserId { get; private set; } = default!;
	public MovieId MovieId { get; private set; } = default!;
	public Score Score { get; private set; } = default!;
	public DateTimeOffset CreatedAtUtc { get; private set; } = default!;

	private Review() { }

	private Review(
		ReviewId reviewId,
		UserId userId,
		Score score,
		DateTimeOffset createdAtUtc)
	{
		Id = reviewId;
		UserId = userId;
		Score = score;
		CreatedAtUtc = createdAtUtc;
	}

	public static Review Create(UserId userId, Score score, DateTimeOffset currentUtcTime)
	{
		return new(
			ReviewId.Create(Guid.NewGuid()).Value,
			userId,
			score,
			currentUtcTime);
	}

	internal void UpdateScore(Score score)
	{
		Score = score;
	}
}
