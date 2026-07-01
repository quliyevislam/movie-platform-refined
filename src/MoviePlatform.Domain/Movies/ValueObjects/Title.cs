using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record Title
{
	public string Value { get; } = default!;

	private Title() { }

	private Title(string value) => Value = value;

	public static Result<Title> Create(string? value)
	{
		string? trimmedValue = value?.Trim();

		if (string.IsNullOrEmpty(trimmedValue))
		{
			return Result.Failure<Title>(MovieErrors.Title.Required);
		}

		if (trimmedValue.Length > MovieConstants.Title.MaxLength)
		{
			return Result.Failure<Title>(MovieErrors.Title.TooLong);
		}

		return Result.Success<Title>(new(trimmedValue));
	}

	public static Title FromPersistence(string value)
	{
		return new(value);
	}
}
