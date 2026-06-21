using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Application.Common.Authentication;

public interface IPasswordHasher
{
	PasswordHash Hash(Password password);
	bool Verify(Password password, PasswordHash asswordHash);
}
