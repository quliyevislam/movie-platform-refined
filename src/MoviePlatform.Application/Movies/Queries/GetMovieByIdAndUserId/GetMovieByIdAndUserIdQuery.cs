using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Users;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetMovieByIdAndUserId;

public record GetMovieByIdAndUserIdQuery(Guid UserId, Guid MovieId) : IQuery<MovieResponse>;
