using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies.Enums;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record Genre
{
	public GenreType Value { get; } = GenreType.Unknown;

	private Genre() { }

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
			return Result.Failure<Genre>(MovieErrors.Genre.OutOfBounds);
		}

		return Result.Success<Genre>(new(genreType));
	}

	public static Genre FromPersistence(string value)
	{
		string trimmedValue = value.Trim();

		if (!Enum.TryParse<GenreType>(trimmedValue, true, out var genreType) || !Enum.IsDefined(typeof(GenreType), genreType))
		{
			return new(GenreType.Unknown);
		}

		return new(genreType);
	}
}
