using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Movies.Commands.AddComment;

public record AddCommentCommand(Guid UserId, Guid MovieId, string Content) : ICommand<Guid>;
