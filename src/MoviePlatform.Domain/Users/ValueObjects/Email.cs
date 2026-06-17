using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Users;
using System.Net.Mail;

namespace MoviePlatform.Domain.Users.ValueObjects;

public readonly record struct Email
{
	public string Value { get; }

	public Email()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private Email(string value) => Value = value;

	public static Result<Email> Create(string? value)
	{
		if (value is null)
		{
			return Result.Failure<Email>(UserErrors.Email.Required);
		}

		string trimmedValue = value.Trim();

		if (trimmedValue.Length == 0)
		{
			return Result.Failure<Email>(UserErrors.Email.Empty);
		}

		if (trimmedValue.Length > UserConstants.Email.MaxLength)
		{
			return Result.Failure<Email>(UserErrors.Email.TooLong);
		}

		if (!MailAddress.TryCreate(trimmedValue, out _))
		{
			return Result.Failure<Email>(UserErrors.Email.InvalidFormat);
		}

		return Result.Success<Email>(new(trimmedValue));
	}
}
