using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetMoviesPagedByUserId;

public record GetMoviesPagedByUserIdQuery(Guid UserId, int Page, int PageSize) : IQuery<PagedList<MovieResponse>>;
