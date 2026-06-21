using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Movies.Commands.CreateMovie;

public record CreateMovieCommand(
	Guid UserId,
	string Title,
	string? Description,
	string? Genre,
	DateOnly ReleaseDate) : ICommand<Guid>;
