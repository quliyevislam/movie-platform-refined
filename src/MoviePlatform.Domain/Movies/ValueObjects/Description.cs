using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record Description
{
	public string? Value { get; }

	private Description() { }

	private Description(string? value) => Value = value;

	public static Result<Description> Create(string? value)
	{
		string? trimmedValue = value?.Trim();

		if (trimmedValue?.Length > MovieConstants.Description.MaxLength)
		{
			return Result.Failure<Description>(MovieErrors.Description.TooLong);
		}

		return Result.Success<Description>(new(trimmedValue));
	}

	public static Description FromPersistence(string? value)
	{
		return new(value);
	}
}
