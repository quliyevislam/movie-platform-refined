using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct Title
{
	public string Value { get; }

	public Title()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private Title(string value) => Value = value;

	public static Result<Title> Create(string? value)
	{
		if (value is null)
		{
			return Result.Failure<Title>(MovieErrors.Title.Required);
		}

		string trimmedValue = value.Trim();

		if (trimmedValue.Length == 0)
		{
			return Result.Failure<Title>(MovieErrors.Title.Empty);
		}

		if (trimmedValue.Length > MovieConstants.Title.MaxLength)
		{
			return Result.Failure<Title>(MovieErrors.Title.TooLong);
		}

		return Result.Success<Title>(new(trimmedValue));
	}
}
