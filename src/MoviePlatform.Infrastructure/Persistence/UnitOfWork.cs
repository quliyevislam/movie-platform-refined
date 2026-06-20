using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
	private readonly MoviePlatformDbContext _context;

	public UnitOfWork(MoviePlatformDbContext context)
	{
		_context = context;
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return await _context.SaveChangesAsync(cancellationToken);
	}
}
