using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Users;

public interface IUserWriteRepository
{
	Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);
	Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
	Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
	void Add(User user);
	void Remove(User user);
}
