using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Users.ValueObjects;

public record PasswordHash
{
	public string Value { get; } = default!;

	private PasswordHash() { }

	private PasswordHash(string value) => Value = value;

	public static Result<PasswordHash> Create(string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return Result.Failure<PasswordHash>(UserErrors.PasswordHash.Required);
		}

		return Result.Success<PasswordHash>(new(value));
	}

	public static PasswordHash FromPersistence(string value)
	{
		return new(value);
	}
}
