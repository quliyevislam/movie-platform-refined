using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetCommentsPagedByMovieId;

public record GetCommentsPagedByMovieIdQuery(
	Guid MovieId,
	int Page,
	int PageSize) : IQuery<PagedList<CommentResponse>>;
