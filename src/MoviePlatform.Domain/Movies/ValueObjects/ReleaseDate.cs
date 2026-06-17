using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct ReleaseDate
{
	public DateOnly Value { get; }

	public ReleaseDate()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private ReleaseDate(DateOnly value) => Value = value;

	public static Result<ReleaseDate> Create(DateOnly value, DateTimeOffset currentUtcTime)
	{
		if (value > DateOnly.FromDateTime(currentUtcTime.DateTime))
		{
			return Result.Failure<ReleaseDate>(MovieErrors.ReleaseDate.InFuture);
		}

		return Result.Success<ReleaseDate>(new(value));
	}
}
