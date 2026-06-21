using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetMoviesPaged;

public record GetMoviesPagedQuery(int Page, int PageSize) : IQuery<PagedList<MovieResponse>>;
