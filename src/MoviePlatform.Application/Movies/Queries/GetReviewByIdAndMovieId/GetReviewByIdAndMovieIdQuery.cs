using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Users;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetReviewByIdAndMovieId;

public record GetReviewByIdAndMovieIdQuery(Guid MovieId, Guid ReviewId) : IQuery<ReviewResponse>;
