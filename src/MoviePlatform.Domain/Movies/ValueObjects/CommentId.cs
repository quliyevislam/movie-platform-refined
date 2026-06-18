using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies.ValueObjects;

public readonly record struct CommentId
{
	public Guid Value { get; }

	public CommentId()
	{
		throw new InvalidOperationException("Instantiation via the default parameterless constructor is prohibited.");
	}

	private CommentId(Guid value) => Value = value;

	public static Result<CommentId> Create(Guid value)
	{
		if (value == Guid.Empty)
		{
			return Result.Failure<CommentId>(MovieErrors.CommentId.Empty);
		}

		return Result.Success<CommentId>(new(value));
	}
}
