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

	public async Task<Result<Guid>> Handle(
		RegisterUserCommand request,
		CancellationToken cancellationToken)
	{
		Result<Name> nameResult = Name.Create(request.Name);
		Result<Email> emailResult = Email.Create(request.Email);
		Result<Password> passwordResult = Password.Create(request.Password);
		Result result = Result.Combine([emailResult, passwordResult, nameResult]);

		if (result.IsFailure)
		{
			return Result.Failure<Guid>(result.Errors);
		}

		bool isEmailUnique = await _userWriteRepository.IsEmailUniqueAsync(
			emailResult.Value,
			cancellationToken);

		if (!isEmailUnique)
		{
			return Result.Failure<Guid>(UserErrors.EmailAlreadyInUse);
		}

		PasswordHash passwordHash = _passwordHasher.Hash(passwordResult.Value);
		User user = User.Create(
			nameResult.Value,
			emailResult.Value,
			passwordHash,
			_timeProvider.GetUtcNow());

		_userWriteRepository.Add(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success(user.Id.Value);
	}
}
