using MoviePlatform.Domain.Common;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Users;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetCommentByIdAndMovieId;

public record GetCommentByIdAndMovieIdQuery(Guid MovieId, Guid CommentId) : IQuery<CommentResponse>;