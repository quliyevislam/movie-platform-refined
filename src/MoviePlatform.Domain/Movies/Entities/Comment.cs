using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Movies.Entities;

public sealed class Comment : BaseEntity<CommentId>
{
	public UserId UserId { get; private set; }
	public MovieId MovieId { get; private set; }
	public Content Content { get; private set; }
	public DateTimeOffset CreatedAtUtc { get; private set; }

	private Comment(
		CommentId commentId,
		UserId userId,
		Content content,
		DateTimeOffset createdAtUtc) : base(commentId)
	{
		UserId = userId;
		Content = content;
		CreatedAtUtc = createdAtUtc;
	}

	public static Result<Comment> Create(Guid userId, string? content, DateTimeOffset currentUtcTime)
	{
		Result<UserId> userIdResult = UserId.Create(userId);

		if (userIdResult.IsFailure)
		{
			return Result.Failure<Comment>(userIdResult.Errors);
		}

		Result<Content> contentResult = Content.Create(content);

		if (contentResult.IsFailure)
		{
			return Result.Failure<Comment>(contentResult.Errors);
		}

		return Result.Success<Comment>(new(
			CommentId.Create(Guid.NewGuid()).Value,
			userIdResult.Value,
			contentResult.Value,
			currentUtcTime));
	}
}
