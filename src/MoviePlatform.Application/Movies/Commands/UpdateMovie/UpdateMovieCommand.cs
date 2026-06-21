using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Movies.Commands.UpdateMovie;

public record UpdateMovieCommand(
	Guid UserId,
	Guid MovieId,
	string Title,
	string? Description,
	string? Genre,
	DateOnly ReleaseDate) : ICommand<Guid>;
