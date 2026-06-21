using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies.Entities;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Domain.Movies;

public sealed class Movie : AggregateRoot<MovieId>
{
	public UserId UserId { get; private set; }
	public Title Title { get; private set; }
	public Description Description { get; private set; }
	public Genre Genre { get; private set; }
	public ReleaseDate ReleaseDate { get; private set; }
	public AverageScore AverageScore { get; private set; }
	public ReviewCount ReviewCount { get; private set; }
	public DateTimeOffset CreatedAtUtc { get; private set; }

	private readonly List<Review> _reviews = [];
	private readonly List<Comment> _comments = [];

	public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
	public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

	private Movie(MovieId movieId) : base(movieId) { }

	private Movie(
		MovieId movieId,
		UserId userId,
		Title title,
		Description description,
		Genre genre,
		ReleaseDate releaseDate,
		DateTimeOffset createdAtUtc) : base(movieId)
	{
		UserId = userId;
		Title = title;
		Description = description;
		Genre = genre;
		ReleaseDate = releaseDate;
		AverageScore = AverageScore.Create(0.0D).Value;
		ReviewCount = ReviewCount.Create(0).Value;
		CreatedAtUtc = createdAtUtc;
	}

	public static Movie Create(
		UserId userId,
		Title title,
		Description description,
		Genre genre,
		ReleaseDate releaseDate,
		DateTimeOffset currentUtcTime)
	{
		return new(
			MovieId.Create(Guid.NewGuid()).Value,
			userId,
			title,
			description,
			genre,
			releaseDate,
			currentUtcTime);
	}

	public void Update(
		Title newTitle,
		Description newDescription,
		Genre newGenre,
		ReleaseDate newReleaseDate)
	{
		Title = newTitle;
		Description = newDescription;
		Genre = newGenre;
		ReleaseDate = newReleaseDate;
	}

	private void RecalculateAverageRating()
	{
		int totalReviewCount = _reviews.Count;

		ReviewCount = ReviewCount.Create(totalReviewCount).Value;

		if (totalReviewCount == 0)
		{
			AverageScore = AverageScore.Create(0.0D).Value;
			return;
		}

		double totalScoreSum = _reviews.Sum(review => review.Score.Value);
		AverageScore = AverageScore.Create(totalScoreSum / totalReviewCount).Value;
	}

	public void SubmitReview(Review newReview)
	{
		Review? existingReview = _reviews.FirstOrDefault(review => review.UserId == newReview.UserId);

		if (existingReview is null)
		{
			_reviews.Add(newReview);
		}
		else
		{
			existingReview.UpdateScore(newReview.Score);
		}

		RecalculateAverageRating();
	}

	public void AddComment(Comment newComment)
	{
		_comments.Add(newComment);
	}

	public Result DeleteCommentForUser(CommentId commentId, UserId userId)
	{
		Comment? existingComment = _comments.FirstOrDefault(comment => comment.Id == commentId);

		if (existingComment is null)
		{
			return Result.Failure(MovieErrors.Comment.NotFound);
		}

		if (existingComment.UserId != userId)
		{
			return Result.Failure(MovieErrors.Comment.Forbidden);
		}

		_comments.Remove(existingComment);

		return Result.Success();
	}
}
