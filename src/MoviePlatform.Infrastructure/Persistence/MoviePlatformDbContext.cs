using Microsoft.EntityFrameworkCore;
using MoviePlatform.Domain.Users;
using MoviePlatform.Domain.Movies;

namespace MoviePlatform.Infrastructure.Persistence;

public sealed class MoviePlatformDbContext : DbContext
{
	public DbSet<User> Users => Set<User>();
	public DbSet<Movie> Movies => Set<Movie>();

	public MoviePlatformDbContext(DbContextOptions<MoviePlatformDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoviePlatformDbContext).Assembly);
	}
}
