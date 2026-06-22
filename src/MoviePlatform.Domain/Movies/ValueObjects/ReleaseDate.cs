using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record ReleaseDate
{
	public DateOnly Value { get; }

	private ReleaseDate() { }

	private ReleaseDate(DateOnly value) => Value = value;

	public static Result<ReleaseDate> Create(DateOnly value, DateTimeOffset currentUtcTime)
	{
		if (value > DateOnly.FromDateTime(currentUtcTime.DateTime))
		{
			return Result.Failure<ReleaseDate>(MovieErrors.ReleaseDate.InFuture);
		}

		return Result.Success<ReleaseDate>(new(value));
	}

	public static ReleaseDate FromPersistence(DateOnly value)
	{
		return new(value);
	}
}
