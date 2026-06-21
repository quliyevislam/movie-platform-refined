using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Users;

public sealed class User : AggregateRoot<UserId>
{
	public Name Name { get; private set; }
	public Email Email { get; private set; }
	public PasswordHash PasswordHash { get; private set; }
	public DateTimeOffset CreatedAtUtc { get; private set; }

	private User(UserId userId) : base(userId) { }

	private User(
		UserId userId,
		Name name,
		Email email,
		PasswordHash passwordHash,
		DateTimeOffset createdAtUtc) : base(userId)
	{
		Name = name;
		Email = email;
		PasswordHash = passwordHash;
		CreatedAtUtc = createdAtUtc;
	}

	public static User Create(
		Name name,
		Email email,
		PasswordHash passwordHash,
		DateTimeOffset createdAtUtc)
	{
		return new(
			UserId.Create(Guid.NewGuid()).Value,
			name,
			email,
			passwordHash,
			createdAtUtc);
	}
}
