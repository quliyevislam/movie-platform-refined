using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Movies.Entities;

public sealed class Comment : BaseEntity<CommentId>
{
	public UserId UserId { get; private set; } = default!;
	public MovieId MovieId { get; private set; } = default!;
	public Content Content { get; private set; } = default!;
	public DateTimeOffset CreatedAtUtc { get; private set; } = default!;

	private Comment() { }

	private Comment(
		CommentId commentId,
		UserId userId,
		Content content,
		DateTimeOffset createdAtUtc)
	{
		Id = commentId;
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

	public void UpdateContent(Content content)
	{
		Content = content;
	}
}
