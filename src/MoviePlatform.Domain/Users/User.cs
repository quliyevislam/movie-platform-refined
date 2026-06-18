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

	public static Result<User> Create(
		string? name,
		string? email,
		string? passwordHash,
		DateTimeOffset createdAtUtc)
	{
		Result<Name> nameResult = Name.Create(name);
		Result<Email> emailResult = Email.Create(email);
		Result<PasswordHash> passwordHashResult = PasswordHash.Create(passwordHash);
		Result result = Result.Combine([nameResult, emailResult, passwordHashResult]);

		return result.IsFailure ? Result.Failure<User>(result.Errors) : Result.Success<User>(new(
			UserId.Create(Guid.NewGuid()).Value,
			nameResult.Value,
			emailResult.Value,
			passwordHashResult.Value,
			createdAtUtc));
	}
}
