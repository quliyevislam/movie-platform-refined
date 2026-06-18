using Microsoft.EntityFrameworkCore;
using MoviePlatform.Domain.Users;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Infrastructure.Persistence.Repositories;

public sealed class UserWriteRepository : IUserWriteRepository
{
	private readonly MoviePlatformDbContext _context;

	public UserWriteRepository(MoviePlatformDbContext context)
	{
		_context = context;
	}

	public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

	public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
	{
		return !await _context.Users.AnyAsync(user => user.Email == email, cancellationToken);
	}

	public void Add(User user) => _context.Users.Add(user);
	public void Remove(User user) => _context.Users.Remove(user);
}
