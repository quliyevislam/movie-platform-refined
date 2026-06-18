using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct AverageScore
{
	public double Value { get; }

	public AverageScore()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private AverageScore(double value) => Value = value;

	public static Result<AverageScore> Create(double value)
	{
		if (value == 0)
		{
			return Result.Success<AverageScore>(new(value));
		}

		double roundedValue = Math.Round(value, MovieConstants.AverageScore.DecimalPlacesScale);

		if (roundedValue < MovieConstants.AverageScore.MinScore || roundedValue > MovieConstants.AverageScore.MaxScore)
		{
			return Result.Failure<AverageScore>(MovieErrors.AverageScore.OutOfRange);
		}

		return Result.Success<AverageScore>(new(roundedValue));
	}

	public static AverageScore FromPersistence(double value)
	{
		return new(value);
	}
}
