namespace MoviePlatform.Domain.Users;

public static class UserConstants
{
	public static class Name
	{
		public static readonly int MaxLength = 50;
	}

	public static class Email
	{
		public static readonly int MaxLength = 254;
	}

	public static class Password
	{
		public static readonly int MinLength = 8;

		public static readonly string RequireUppercase = @"[A-Z]";
		public static readonly string RequireLowercase = @"[a-z]";
		public static readonly string RequireDigit = @"[0-9]";
		public static readonly string RequireNoSpace  = @"^\S*$";
	}
}
