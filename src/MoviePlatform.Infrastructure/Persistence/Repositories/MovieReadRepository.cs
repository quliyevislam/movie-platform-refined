using Dapper;
using System.Data;
using MoviePlatform.Application.Movies;
using MoviePlatform.Application.Common.Data;
using MoviePlatform.Domain.Common;

namespace MoviePlatform.Infrastructure.Persistence.Repositories;

public sealed class MovieReadRepository : IMovieReadRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;

	public MovieReadRepository(ISqlConnectionFactory sqlConnectionFactory)
	{
		_sqlConnectionFactory = sqlConnectionFactory;
	}

	public async Task<PagedList<MovieResponse>> GetByUserIdAsync(
			Guid userId,
			int page,
			int pageSize,
			CancellationToken cancellationToken = default)
	{
		using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

		const string sql = """
			SELECT
				movie_id		AS MovieId,
				user_id			AS UserId,
				title			AS Title,
				description		AS Description,
				genre			AS Genre,
				release_date	AS ReleaseDate,
				average_score	AS AverageScore,
				review_count	AS ReviewCount,
				created_at_utc	AS CreatedAtUtc,
				COUNT(*) OVER()	AS TotalCount
			FROM movies
			WHERE user_id = @UserId
			ORDER BY created_at_utc DESC
			LIMIT @PageSize OFFSET @Offset;
			""";

		var parameters = new
		{
			UserId = userId,
			PageSize = pageSize,
			Offset = (page - 1) * pageSize
		};

		IEnumerable<MovieReadRow> rows = await connection.QueryAsync<MovieReadRow>(sql, parameters);

		List<MovieReadRow> rowList = rows.ToList();

		int totalCount = rowList.Count > 0 ? rowList[0].TotalCount : 0;

		List<MovieResponse> movies = rowList
			.Select(row => new MovieResponse(
				row.MovieId,
				row.UserId,
				row.Title,
				row.Description,
				row.Genre,
				row.ReleaseDate,
				row.AverageScore,
				row.ReviewCount,
				row.CreatedAtUtc))
			.ToList();

		return PagedList<MovieResponse>.Create(movies, page, pageSize, totalCount);
	}
}

sealed record MovieReadRow(
	Guid MovieId,
	Guid UserId,
	string Title,
	string? Description,
	string Genre,
	DateOnly ReleaseDate,
	double AverageScore,
	int ReviewCount,
	DateTimeOffset CreatedAtUtc,
	int TotalCount);
