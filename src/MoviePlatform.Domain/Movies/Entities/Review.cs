using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Movies.Entities;

public sealed class Review : BaseEntity<ReviewId>
{
	public UserId UserId { get; private set; }
	public MovieId MovieId { get; private set; }
	public Score Score { get; private set; }
	public DateTimeOffset CreatedAtUtc { get; private set; }

	private Review(
		ReviewId reviewId,
		UserId userId,
		Score score,
		DateTimeOffset createdAtUtc) : base(reviewId)
	{
		UserId = userId;
		Score = score;
		CreatedAtUtc = createdAtUtc;
	}

	public static Result<Review> Create(Guid userId, int score)
	{
		Result<UserId> userIdResult = UserId.Create(userId);

		if (userIdResult.IsFailure)
		{
			return Result.Failure<Review>(userIdResult.Errors);
		}

		Result<Score> scoreResult = Score.Create(score);

		if (scoreResult.IsFailure)
		{
			return Result.Failure<Review>(scoreResult.Errors);
		}

		return Result.Success<Review>(new(
			ReviewId.Create(Guid.NewGuid()).Value,
			userIdResult.Value,
			scoreResult.Value,
			DateTimeOffset.UtcNow));
	}

	internal void UpdateScore(Score newScore)
	{
		Score = newScore;
	}
}
