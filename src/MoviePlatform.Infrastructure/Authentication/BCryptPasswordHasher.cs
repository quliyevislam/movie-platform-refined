using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Authentication;
using BCryptNet = BCrypt.Net.BCrypt;

namespace MoviePlatform.Infrastructure.Authentication;

internal sealed class BCryptPasswordHasher : IPasswordHasher
{
	public PasswordHash Hash(Password password)
	{
		return PasswordHash.Create(BCryptNet.HashPassword(password.Value)).Value;
	}

	public bool Verify(Password password, PasswordHash passwordHash)
	{
		return BCryptNet.Verify(password.Value, passwordHash.Value);
	}
}
