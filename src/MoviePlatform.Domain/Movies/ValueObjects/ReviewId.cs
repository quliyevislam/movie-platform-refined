using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record ReviewId
{
	public Guid Value { get; }

	private ReviewId() { }

	private ReviewId(Guid value) => Value = value;

	public static Result<ReviewId> Create(Guid value)
	{
		if (value == Guid.Empty)
		{
			return Result.Failure<ReviewId>(MovieErrors.ReviewId.Empty);
		}

		return Result.Success<ReviewId>(new(value));
	}

	public static ReviewId FromPersistence(Guid value)
	{
		return new(value);
	}
}
