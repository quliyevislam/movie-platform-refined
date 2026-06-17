using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct Description
{
	public string? Value { get; }

	public Description()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

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
}
