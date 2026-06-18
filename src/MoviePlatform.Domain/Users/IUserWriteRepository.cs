using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Users;

public interface IUserWriteRepository
{
	Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);
	Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
	void Delete(User user);
	void Add(User user);
	void Update(User user);
}
