using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
