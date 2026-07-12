using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Movies.Commands.DeleteComment;

public record DeleteCommentCommand(Guid UserId, Guid MovieId, Guid CommentId) : ICommand;
