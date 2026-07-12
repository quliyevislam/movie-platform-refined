namespace MoviePlatform.Application.Movies;

public record CommentResponse
{
	public Guid CommentId { get; private set; }
	public Guid UserId { get; private set; }
	public Guid MovieId { get; private set; }
	public string Content { get; private set; } = default!;
	public DateTimeOffset CreatedAtUtc { get; private set; }

	public CommentResponse(
		Guid commentId,
		Guid userId,
		Guid movieId,
		string content,
		DateTimeOffset createdAtUtc)
	{
		CommentId = commentId;
		UserId = userId;
		MovieId = movieId;
		Content = content;
		CreatedAtUtc = createdAtUtc;
	}

	private CommentResponse() {}
}
