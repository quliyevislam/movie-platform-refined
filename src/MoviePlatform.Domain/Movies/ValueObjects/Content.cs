using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct Content
{
	public string Value { get; }

	public Content()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

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
}
