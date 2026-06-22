using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Users.ValueObjects;

public record UserId
{
	public Guid Value { get; } = default!;

	private UserId() { }

	private UserId(Guid value) => Value = value;

	public static Result<UserId> Create(Guid value)
	{
		if (value == Guid.Empty)
		{
			return Result.Failure<UserId>(UserErrors.UserId.Empty);
		}

		return Result.Success<UserId>(new(value));
	}

	public static UserId FromPersistence(Guid value)
	{
		return new(value);
	}
}
