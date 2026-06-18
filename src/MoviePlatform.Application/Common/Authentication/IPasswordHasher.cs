using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Application.Common.Authentication;

public interface IPasswordHasher
{
	string Hash(Password password);
	bool Verify(Password password, PasswordHash asswordHash);
}
