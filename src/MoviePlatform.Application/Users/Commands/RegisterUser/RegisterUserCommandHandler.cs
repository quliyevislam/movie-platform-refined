using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Users;
using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Authentication;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Application.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
	private readonly IUserWriteRepository _userWriteRepository;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IUnitOfWork _unitOfWork;
	private readonly TimeProvider _timeProvider;

	public RegisterUserCommandHandler(
		IUserWriteRepository userWriteRepository,
		IPasswordHasher passwordHasher,
		IUnitOfWork unitOfWork,
		TimeProvider timeProvider)
	{
		_userWriteRepository = userWriteRepository;
		_passwordHasher = passwordHasher;
		_unitOfWork = unitOfWork;
		_timeProvider = timeProvider;
	}

	public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		Result<Email> emailResult = Email.Create(request.Email);
		Result<Password> passwordResult = Password.Create(request.Password);
		Result result = Result.Combine([emailResult, passwordResult]);

		if (result.IsFailure)
		{
			return Result.Failure<Guid>(result.Errors);
		}

		bool isEmailUnique = await _userWriteRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken);

		if (!isEmailUnique)
		{
			return Result.Failure<Guid>(UserErrors.EmailAlreadyInUse);
		}

		string passwordHash = _passwordHasher.Hash(passwordResult.Value);
		Result<User> userResult = User.Create(request.Name, request.Email, passwordHash, _timeProvider.GetUtcNow());

		if (userResult.IsFailure)
		{
			return Result.Failure<Guid>(userResult.Errors);
		}

		_userWriteRepository.Add(userResult.Value);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success(userResult.Value.Id.Value);
	}
}
