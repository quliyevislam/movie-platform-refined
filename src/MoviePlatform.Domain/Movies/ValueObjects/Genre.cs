using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies.Enums;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct Genre
{
	public GenreType Value { get; }

	public Genre()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private Genre(GenreType value)
	{
		Value = value;
	}

	public static Result<Genre> Create(string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return Result.Success<Genre>(new(GenreType.Unknown));
		}

		string trimmedValue = value.Trim();

		if (!Enum.TryParse<GenreType>(trimmedValue, true, out var genreType) || !Enum.IsDefined(typeof(GenreType), genreType))
		{
			return Result.Failure<Genre>(MovieErrors.Genre.Invalid);
		}

		return Result.Success<Genre>(new(genreType));
	}

	public static Genre FromPersistence(GenreType value)
	{
		return new(value);
	}
}
