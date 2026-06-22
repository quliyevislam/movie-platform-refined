using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Users.ValueObjects;

public record Name
{
	public string Value { get; } = default!;

	private Name() { }

	private Name(string value) => Value = value;

	public static Result<Name> Create(string? value)
	{
		if (value is null)
		{
			return Result.Failure<Name>(UserErrors.Name.Required);
		}

		string trimmedValue = value.Trim();

		if (trimmedValue.Length == 0)
		{
			return Result.Failure<Name>(UserErrors.Name.Empty);
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
