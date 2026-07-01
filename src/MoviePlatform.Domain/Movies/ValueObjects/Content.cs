using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record Content
{
	public string Value { get; } = default!;

	private Content() { }

	private Content(string value) => Value = value;

	public static Result<Content> Create(string? value)
	{
			string? trimmedValue = value?.Trim();

			if (string.IsNullOrEmpty(trimmedValue))
			{
				return Result.Failure<Content>(MovieErrors.Content.Required);
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
