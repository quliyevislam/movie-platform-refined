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

	public async Task<MovieResponse?> GetByIdAndUserIdAsync(
		Guid movieId,
		Guid userId,
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
				created_at_utc	AS CreatedAtUtc
			FROM movies
			WHERE movie_id = @MovieId AND user_id = @UserId;
			""";

		var parameters = new { MovieId = movieId, UserId = userId };

		CommandDefinition command = new CommandDefinition(
			sql,
			parameters,
			cancellationToken: cancellationToken);

		return await connection.QuerySingleOrDefaultAsync<MovieResponse>(command);
	}

	public async Task<MovieResponse?> GetByIdAsync(
		Guid movieId,
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
				created_at_utc	AS CreatedAtUtc
			FROM movies
			WHERE movie_id = @MovieId;
			""";

		var parameters = new { MovieId = movieId };

		CommandDefinition command = new CommandDefinition(
			sql,
			parameters,
			cancellationToken: cancellationToken);

		return await connection.QuerySingleOrDefaultAsync<MovieResponse>(command);
	}

	public async Task<PagedList<MovieResponse>> GetPagedByUserIdAsync(
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

		CommandDefinition command = new CommandDefinition(
			sql,
			parameters,
			cancellationToken: cancellationToken);
		IEnumerable<MovieReadRow> rows = await connection.QueryAsync<MovieReadRow>(command);

		return MapToMovieResponsePagedList(rows.ToList(), page, pageSize);
	}

	public async Task<PagedList<MovieResponse>> GetPagedAsync(
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
			ORDER BY created_at_utc DESC
			LIMIT @PageSize OFFSET @Offset;
			""";

		var parameters = new
		{
			PageSize = pageSize,
			Offset = (page - 1) * pageSize
		};

		CommandDefinition command = new CommandDefinition(
			sql,
			parameters,
			cancellationToken: cancellationToken);
		IEnumerable<MovieReadRow> rows = await connection.QueryAsync<MovieReadRow>(command);

		return MapToMovieResponsePagedList(rows.ToList(), page, pageSize);
	}

	
	public async Task<PagedList<ReviewResponse>> GetReviewsPagedByMovieIdAsync(
		Guid movieId,
		int page,
		int pageSize,
		CancellationToken cancellationToken = default)
	{		
		using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

		const string sql = """
			SELECT
				review_id		AS ReviewId,
				user_id			AS UserId,
				movie_id		AS MovieId,
				score			AS Score,
				created_at_utc	AS CreatedAtUtc,
				COUNT(*) OVER()	AS TotalCount
			FROM reviews
			WHERE movie_id = @MovieId
			ORDER BY created_at_utc DESC
			LIMIT @PageSize OFFSET @Offset;
			""";

		var parameters = new
		{
			MovieId = movieId,
			PageSize = pageSize,
			Offset = (page - 1) * pageSize
		};
		
		CommandDefinition command = new CommandDefinition(
			sql,
			parameters,
			cancellationToken: cancellationToken);
		IEnumerable<ReviewReadRow> rows = await connection.QueryAsync<ReviewReadRow>(command);

		return MapToReviewResponsePagedList(rows.ToList(), page, pageSize);
	}

	private static PagedList<MovieResponse> MapToMovieResponsePagedList(
		List<MovieReadRow> rowList,
		int page,
		int pageSize)
	{
		long totalCount = rowList.Count > 0 ? rowList[0].TotalCount : 0;
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

	private static PagedList<ReviewResponse> MapToReviewResponsePagedList(
		List<ReviewReadRow> rowList,
		int page,
		int pageSize)
	{
		long totalCount = rowList.Count > 0 ? rowList[0].TotalCount : 0;
		List<ReviewResponse> reviews = rowList
			.Select(row => new ReviewResponse(
				row.ReviewId,
				row.UserId,
				row.MovieId,
				row.Score,
				row.CreatedAtUtc))
			.ToList();

		return PagedList<ReviewResponse>.Create(reviews, page, pageSize, totalCount);
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
	DateTime CreatedAtUtc,
	long TotalCount);

sealed record ReviewReadRow(
	Guid ReviewId,
	Guid UserId,
	Guid MovieId,
	int Score,
	DateTime CreatedAtUtc,
	long TotalCount);
