using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Users.ValueObjects;

public record Name
{
	public string Value { get; } = default!;

	private Name() { }

	private Name(string value) => Value = value;

	public static Result<Name> Create(string? value)
	{
		string? trimmedValue = value?.Trim();

		if (string.IsNullOrEmpty(trimmedValue))
		{
			return Result.Failure<Name>(UserErrors.Name.Required);
		}

		if (trimmedValue.Length > UserConstants.Name.MaxLength)
		{
			return Result.Failure<Name>(UserErrors.Name.TooLong);
		}

		return Result.Success<Name>(new(trimmedValue));
	}

	public static Name FromPersistence(string value)
	{
		return new(value);
	}
}
