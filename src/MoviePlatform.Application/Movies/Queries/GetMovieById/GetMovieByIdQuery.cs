using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Users;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetMovieById;

public record GetMovieByIdQuery(Guid MovieId) : IQuery<MovieResponse>;
