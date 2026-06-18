using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Users;
using System.Text.RegularExpressions;

namespace MoviePlatform.Domain.Users.ValueObjects;

public readonly record struct Password
{
	public string Value { get; }

	public Password()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private Password(string value) => Value = value;

	public static Result<Password> Create(string? value)
	{
		if (value is null)
		{
			return Result.Failure<Password>(UserErrors.Password.Required);
		}

		if (value.Length == 0)
		{
			return Result.Failure<Password>(UserErrors.Password.Empty);
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

		if (errors.Count != 0)
		{
			return Result.Failure<Password>(errors);
		}

		return Result.Success<Password>(new(value));
	}
}
