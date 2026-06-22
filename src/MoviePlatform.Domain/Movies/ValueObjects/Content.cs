using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record Content
{
	public string Value { get; } = default!;

	private Content() { }

	private Content(string value) => Value = value;

	public static Result<Content> Create(string? value)
	{
			if (value is null)
			{
				return Result.Failure<Content>(MovieErrors.Content.Required);
			}

			string trimmedValue = value.Trim();

			if (trimmedValue.Length == 0)
			{
				return Result.Failure<Content>(MovieErrors.Content.Empty);
			}

			if (trimmedValue.Length > MovieConstants.Content.MaxLength)
			{
				return Result.Failure<Content>(MovieErrors.Content.TooLong);
			}

			return Result.Success<Content>(new(trimmedValue));
	}

	public static Content FromPersistence(string value)
	{
		return new(value);
	}
}
