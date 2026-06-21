using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Movies.Commands.DeleteMovie;

public record DeleteMovieCommand(Guid UserId, Guid MovieId) : ICommand;
