using MoviePlatform.Domain.Common;
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

		List<Error> errors = new();

		if (trimmedValue.Length > UserConstants.Email.MaxLength)
		{
			errors.Add(UserErrors.Email.TooLong);
		}

		if (!MailAddress.TryCreate(trimmedValue, out _))
		{
			errors.Add(UserErrors.Email.InvalidFormat);
		}

		return errors.Count == 0 ? Result.Success<Email>(new(trimmedValue)) : Result.Failure<Email>(errors);
	}

	public static Email FromPersistence(string value)
	{
		return new(value);
	}
}
