using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record Score
{
	public int Value { get; }

	private Score() { }

	private Score(int value) => Value = value;

	public static Result<Score> Create(int value)
	{
		if (value < MovieConstants.Score.MinScore || value > MovieConstants.Score.MaxScore)
		{
			return Result.Failure<Score>(MovieErrors.Score.OutOfRange);
		}

		return Result.Success<Score>(new(value));
	}

	public static Score FromPersistence(int value)
	{
		return new(value);
	}
}
