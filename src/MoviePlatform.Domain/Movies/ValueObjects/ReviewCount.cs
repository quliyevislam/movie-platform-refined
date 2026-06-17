using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct ReviewCount
{
	public int Value { get; }

	public ReviewCount()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private ReviewCount(int value) => Value = value;

	public static Result<ReviewCount> Create(int value)
	{
		if (value < 0)
		{
			return Result.Failure<ReviewCount>(MovieErrors.ReviewCount.Negative);
		}

		return Result.Success<ReviewCount>(new(value));
	}
}
