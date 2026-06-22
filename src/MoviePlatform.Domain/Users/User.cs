using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Users;

public sealed class User : AggregateRoot<UserId>
{
	public Name Name { get; private set; } = default!;
	public Email Email { get; private set; } = default!;
	public PasswordHash PasswordHash { get; private set; } = default!;
	public DateTimeOffset CreatedAtUtc { get; private set; } = default!;

	private User() { }

	private User(
		UserId userId,
		Name name,
		Email email,
		PasswordHash passwordHash,
		DateTimeOffset createdAtUtc)
	{
		Id = userId;
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
