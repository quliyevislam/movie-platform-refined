using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Name, string Email, string Password) : ICommand<Guid>;
