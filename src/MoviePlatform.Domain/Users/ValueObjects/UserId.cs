using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Users.ValueObjects;

public readonly record struct UserId
{
	public Guid Value { get; }

	public UserId()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private UserId(Guid value) => Value = value;

	public static Result<UserId> Create(Guid value)
	{
		if (value == Guid.Empty)
		{
			return Result.Failure<UserId>(UserErrors.UserId.Empty);
		}

		return Result.Success<UserId>(new(value));
	}
}
