using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct MovieId
{
	public Guid Value { get; }

	public MovieId()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private MovieId(Guid value) => Value = value;

	public static Result<MovieId> Create(Guid value)
	{
		if (value == Guid.Empty)
		{
			return Result.Failure<MovieId>(MovieErrors.MovieId.Empty);
		}

		return Result.Success<MovieId>(new(value));
	}

	public static MovieId FromPersistence(Guid value)
	{
		return new(value);
	}
}
