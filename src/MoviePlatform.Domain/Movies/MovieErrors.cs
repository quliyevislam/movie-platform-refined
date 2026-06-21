using MoviePlatform.Domain.Common;

namespace MoviePlatform.Domain.Movies;

public static class MovieErrors
{
	public static readonly Error NotFound = Error.NotFound(
		"Movie.NotFound",
		"The movie with the specified id was not found."
	);

	public static readonly Error Forbidden = Error.Forbidden(
		"Movie.Forbidden",
		"You do not have permission to modify this movie."
	);

	public static class MovieId
	{
		public static readonly Error InvalidFormat = Error.Validation(
			"Movie.MovieId.InvalidFormat",
			"The movie id format is invalid."
		);

		public static readonly Error Required = Error.Validation(
			"Movie.MovieId.Required",
			"The movie id is required."
		);

		public static readonly Error Empty = Error.Validation(
			"Movie.MovieId.Empty",
			"The movie id cannot be empty"
		);
	}

	public static class Title
	{
		public static readonly Error Required = Error.Validation(
			"Movie.Title.Required",
			"The movie title is required."
		);

		public static readonly Error Empty = Error.Validation(
			"Movie.Title.Empty",
			"The movie title cannot be empty."
		);

		public static readonly Error TooLong = Error.Validation(
			"Movie.Title.TooLong",
			$"The movie title cannot exceed {MovieConstants.Title.MaxLength} characters."
		);
	}

	public static class Description
	{
		public static readonly Error TooLong = Error.Validation(
			"Movie.Description.TooLong",
			$"The movie description cannot exceed {MovieConstants.Description.MaxLength} characters."
		);
	}

	public static class Genre
	{
		public static readonly Error Invalid = Error.Validation(
			"Movie.Genre.Invalid",
			"The genre is not valid. Please check the /api/movies/genres endpoint for a full list of options."
		);

		public static readonly Error OutOfBounds = Error.Validation(
			"Movie.Genre.OutOfBounds",
			"The provided genre option is outside the allowed system boundaries."
		);
	}

	public static class ReleaseDate
	{
		public static readonly Error InFuture = Error.Validation(
			"Movie.ReleaseDate.InFuture",
			"The movie release date cannot be in the future."
		);
	}

	public static class AverageScore
	{
		public static readonly Error OutOfRange = Error.Validation(
			"Movie.AverageScore.OutOfRange",
			$"The average score must be 0 or between {MovieConstants.AverageScore.MinScore} and {MovieConstants.AverageScore.MaxScore}."
		);
	}

	public static class ReviewCount
	{
		public static readonly Error Negative = Error.Validation(
			"Movie.ReviewCount.Negative",
			"The review count cannot be negative."
		);
	}

	public static class ReviewId
	{
		public static readonly Error Invalid = Error.Validation(
			"Movie.ReviewId.Invalid",
			"The review id is invalid."
		);

		public static readonly Error Empty = Error.Validation(
			"Movie.ReviewId.Empty",
			"The review id cannot be empty"
		);
	}

	public static class Score
	{
		public static readonly Error OutOfRange = Error.Validation(
			"Movie.Score.OutOfRange",
			$"The score must be between {MovieConstants.Score.MinScore} and {MovieConstants.Score.MaxScore}."
		);
	}

	public static class Comment
	{
		public static readonly Error NotFound = Error.NotFound(
			"Movie.Comment.NotFound",
			"The comment with the specified id was not found."
		);

		public static readonly Error Forbidden = Error.Forbidden(
			"Movie.Comment.Forbidden",
			"You do not have permission to modify this comment."
		);
	}

	public static class CommentId
	{
		public static readonly Error Invalid = Error.Validation(
			"Movie.CommentId.Invalid",
			"The comment id is invalid."
		);

		public static readonly Error Empty = Error.Validation(
			"Movie.CommentId.Empty",
			"The comment id cannot be empty"
		);
	}

	public static class Content
	{
		public static readonly Error Required = Error.Validation(
			"Movie.Content.Required",
			"The content is required."
		);

		public static readonly Error Empty = Error.Validation(
			"Movie.Content.Empty",
			"The content cannot be empty."
		);

		public static readonly Error TooLong = Error.Validation(
			"Movie.Content.TooLong",
			$"The content cannot exceed {MovieConstants.Content.MaxLength} characters."
		);
	}
}
