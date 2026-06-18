using MoviePlatform.Domain.Users;

namespace MoviePlatform.Application.Common.Authentication;

public interface IJwtProvider
{
	string Generate(User user);
}
