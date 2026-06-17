using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Users;

namespace MoviePlatform.Domain.Users.ValueObjects;

public readonly record struct Name
{
	public string Value { get; }

	public Name()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

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
}
