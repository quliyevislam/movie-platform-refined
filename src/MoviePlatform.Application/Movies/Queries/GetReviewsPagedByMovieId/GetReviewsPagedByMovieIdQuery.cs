using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetReviewsPagedByMovieId;

public record GetReviewsPagedByMovieIdQuery(
	Guid MovieId,
	int Page,
	int PageSize) : IQuery<PagedList<ReviewResponse>>;
