using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Users;

public static class UserErrors
{
	public static readonly Error EmailAlreadyInUse = Error.Conflict(
		"User.EmailAlreadyInUse",
		"The provided email address is already registered.");

    public static readonly Error InvalidCredentials = Error.Failure(
        "User.InvalidCredentials",
        "The provided credentials are incorrect.");

	public static class UserId
	{
		public static readonly Error Invalid = Error.Validation(
			"User.UserId.Invalid",
			"The user id is invalid.");

		public static readonly Error Empty = Error.Validation(
			"User.UserId.Empty",
			"The user id cannot be empty");
	}

	public static class Name
	{
		public static readonly Error Required = Error.Validation(
			"User.Name.Required",
			"The user name is required.");

		public static readonly Error TooLong = Error.Validation(
			"User.Name.TooLong",
			$"The user name cannot exceed {UserConstants.Name.MaxLength} characters.");
	}

	public static class Email
	{
		public static readonly Error Required = Error.Validation(
			"User.Email.Required",
			"The user email is required.");

		public static readonly Error InvalidFormat = Error.Validation(
			"User.Email.InvalidFormat",
			"The user email format is invalid.");

		public static readonly Error TooLong = Error.Validation(
			"User.Email.TooLong",
			$"The user email cannot exceed {UserConstants.Email.MaxLength} characters.");
	}

	public static class Password
	{
		public static readonly Error Required = Error.Validation(
			"User.Password.Required",
			"The password is required.");

		public static readonly Error TooShort = Error.Validation(
			"User.Password.TooShort",
			$"The password must be at least {UserConstants.Password.MinLength} characters long.");

		public static readonly Error UppercaseRequired = Error.Validation(
			"User.Password.UppercaseRequired",
			"The password must contain at least one uppercase letter.");

		public static readonly Error LowercaseRequired = Error.Validation(
			"User.Password.LowercaseRequired",
			"The password must contain at least one lowercase letter.");

		public static readonly Error DigitRequired = Error.Validation(
			"User.Password.DigitRequired",
			"The password must contain at least one number.");

		public static readonly Error SpacesNotAllowed = Error.Validation(
			"User.Password.NoSpaceRequired",
			"The password must not contain spaces."
		);
	}

	public static class PasswordHash
	{
		public static readonly Error Required = Error.Validation(
			"User.PasswordHash.Required",
			"The password hash is required.");
	}
}
