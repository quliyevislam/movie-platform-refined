using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Users;
using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Authentication;
using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Users.Commands.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
	private readonly IUserWriteRepository _userWriteRepository;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IJwtProvider _jwtProvider;

	public LoginCommandHandler(
		IUserWriteRepository userWriteRepository,
		IPasswordHasher passwordHasher,
		IJwtProvider jwtProvider)
	{
		_userWriteRepository = userWriteRepository;
		_passwordHasher = passwordHasher;
		_jwtProvider = jwtProvider;
	}

	public async Task<Result<LoginResponse>> Handle(
		LoginCommand request,
		CancellationToken cancellationToken)
	{
		Result<Email> emailResult = Email.Create(request.Email);
		Result<Password> passwordResult = Password.Create(request.Password);
		Result result = Result.Combine([emailResult, passwordResult]);

		if (result.IsFailure)
		{
			return Result.Failure<LoginResponse>(result.Errors);
		}

		User? user = await _userWriteRepository.GetByEmailAsync(emailResult.Value, cancellationToken);

		if (user is null)
		{
			return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
		}

		bool isPasswordValid = _passwordHasher.Verify(passwordResult.Value, user.PasswordHash);

		if (!isPasswordValid)
		{
			return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
		}

		string token = _jwtProvider.Generate(user);

		return Result.Success(new LoginResponse(token));
	}
}
