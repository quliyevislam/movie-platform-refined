using MoviePlatform.Domain.Common;
using System.Text.RegularExpressions;

namespace MoviePlatform.Domain.Users.ValueObjects;

public record Password
{
	public string Value { get; } = default!;

	private Password() { }

	private Password(string value) => Value = value;

	public static Result<Password> Create(string? value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return Result.Failure<Password>(UserErrors.Password.Required);
		}

		List<Error> errors = new();

		if (!Regex.IsMatch(value, UserConstants.Password.RequireNoSpace))
		{
			errors.Add(UserErrors.Password.SpacesNotAllowed);
		}

		if (value.Length < UserConstants.Password.MinLength)
		{
			errors.Add(UserErrors.Password.TooShort);
		}

		if (!Regex.IsMatch(value, UserConstants.Password.RequireUppercase))
		{
			errors.Add(UserErrors.Password.UppercaseRequired);
		}

		if (!Regex.IsMatch(value, UserConstants.Password.RequireLowercase))
		{
			errors.Add(UserErrors.Password.LowercaseRequired);
		}

		if (!Regex.IsMatch(value, UserConstants.Password.RequireDigit))
		{
			errors.Add(UserErrors.Password.DigitRequired);
		}

		return errors.Count == 0 ? Result.Success<Password>(new(value)) : Result.Failure<Password>(errors);
	}
}
