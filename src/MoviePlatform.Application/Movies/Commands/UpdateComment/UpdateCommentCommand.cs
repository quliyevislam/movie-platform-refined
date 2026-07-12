using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Movies.Commands.UpdateComment;

public record UpdateCommentCommand(Guid UserId, Guid MovieId, Guid CommentId, string Content) : ICommand<Guid>;
