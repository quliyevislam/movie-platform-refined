using MoviePlatform.Domain.Common;

namespace MoviePlatform.Application.Movies;

public interface IMovieReadRepository
{
	Task<MovieResponse?> GetByIdAndUserIdAsync(
		Guid movieId,
		Guid userId,
		CancellationToken cancellationToken = default);

	Task<MovieResponse?> GetByIdAsync(
		Guid movieId,
		CancellationToken cancellationToken = default);

	Task<PagedList<MovieResponse>> GetPagedByUserIdAsync(
		Guid userId,
		int page,
		int pageSize,
		CancellationToken cancellationToken = default);

	Task<PagedList<MovieResponse>> GetPagedAsync(
		int page,
		int pageSize,
		CancellationToken cancellationToken = default);

	Task<PagedList<ReviewResponse>> GetReviewsPagedByMovieIdAsync(
		Guid movieId,
		int page,
		int pageSize,
		CancellationToken cancellationToken = default);

	Task<ReviewResponse?> GetReviewByIdAndMovieIdAsync(
		Guid movieId,
		Guid reviewId,
		CancellationToken cancellationToken = default);

	Task<PagedList<CommentResponse>> GetCommentsPagedByMovieIdAsync(
		Guid movieId,
		int page,
		int pageSize,
		CancellationToken cancellationToken = default);

	Task<CommentResponse?> GetCommentByIdAndMovieIdAsync(
		Guid movieId,
		Guid commentId,
		CancellationToken cancellationToken = default);
}
