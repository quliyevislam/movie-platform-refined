using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Users.ValueObjects;

public readonly record struct PasswordHash
{
	public string Value { get; }

	public PasswordHash()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private PasswordHash(string value) => Value = value;

	public static Result<PasswordHash> Create(string? value)
	{
		if (value is null)
		{
			return Result.Failure<PasswordHash>(UserErrors.PasswordHash.Required);
		}

		if (value.Length == 0)
		{
			return Result.Failure<PasswordHash>(UserErrors.PasswordHash.Empty);
		}

		return Result.Success<PasswordHash>(new(value));
	}
}
