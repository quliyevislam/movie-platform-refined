using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetCommentsPagedByMovieId;

internal sealed class GetCommentsPagedByMovieIdQueryHandler
	: IQueryHandler<GetCommentsPagedByMovieIdQuery, PagedList<CommentResponse>>
{
	private readonly IMovieReadRepository _movieReadRepository;

	public GetCommentsPagedByMovieIdQueryHandler(IMovieReadRepository movieReadRepository)
	{
		_movieReadRepository = movieReadRepository;
	}

	public async Task<Result<PagedList<CommentResponse>>> Handle(
		GetCommentsPagedByMovieIdQuery	request,
		CancellationToken cancellationToken)
	{
		PagedList<CommentResponse> commentsPagedList = await _movieReadRepository.GetCommentsPagedByMovieIdAsync(
			request.MovieId,
			request.Page,
			request.PageSize,
			cancellationToken);

		return Result.Success<PagedList<CommentResponse>>(commentsPagedList);
	}
}
