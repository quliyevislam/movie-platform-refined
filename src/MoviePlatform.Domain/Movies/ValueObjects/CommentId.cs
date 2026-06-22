using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public record CommentId
{
	public Guid Value { get; }

	private CommentId() { }

	private CommentId(Guid value) => Value = value;

	public static Result<CommentId> Create(Guid value)
	{
		if (value == Guid.Empty)
		{
			return Result.Failure<CommentId>(MovieErrors.CommentId.Empty);
		}

		return Result.Success<CommentId>(new(value));
	}

	public static CommentId FromPersistence(Guid value)
	{
		return new(value);
	}
}
