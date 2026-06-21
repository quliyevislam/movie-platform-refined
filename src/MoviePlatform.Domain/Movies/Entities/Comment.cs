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

	public static Comment Create(UserId userId, Content content, DateTimeOffset currentUtcTime)
	{
		return new(
			CommentId.Create(Guid.NewGuid()).Value,
			userId,
			content,
			currentUtcTime);
	}
}
