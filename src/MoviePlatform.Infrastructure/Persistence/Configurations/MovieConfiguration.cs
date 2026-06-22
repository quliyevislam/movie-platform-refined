using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Infrastructure.Persistence.Configurations;

internal sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
	public void Configure(EntityTypeBuilder<Movie> builder)
	{
		builder.ToTable(
			"movies",
			table =>
			{
				table.HasCheckConstraint(
					"CK_movies_title_not_empty",
					"length(title) > 0");

				table.HasCheckConstraint(
					"CK_movies_average_score_range",
					"average_score = 0 OR"
					+ $" (average_score >= {MovieConstants.AverageScore.MinScore} AND"
					+ $" average_score <= {MovieConstants.AverageScore.MaxScore})");

				table.HasCheckConstraint(
					"CK_movies_review_count_not_negative",
					"review_count >= 0");
			});

		builder.HasKey(movie => movie.Id);

		builder
			.Property(movie => movie.Id)
			.HasColumnName("movie_id")
			.HasConversion(
				movieId => movieId.Value,
				value => MovieId.FromPersistence(value));

		builder
			.Property(movie => movie.UserId)
			.HasColumnName("user_id")
			.IsRequired()
			.HasConversion(
				userId => userId.Value,
				value => UserId.FromPersistence(value));

		builder
			.Property(movie => movie.Title)
			.HasColumnName("title")
			.IsRequired()
			.HasMaxLength(MovieConstants.Title.MaxLength)
			.HasConversion(
				title => title.Value,
				value => Title.FromPersistence(value));

		builder
			.Property(movie => movie.Description)
			.HasColumnName("description")
			.IsRequired(false)
			.HasMaxLength(MovieConstants.Description.MaxLength)
			.HasConversion(
				description => description == null ? null : description.Value,
				value => Description.FromPersistence(value));

		builder
			.Property(movie => movie.Genre)
			.HasColumnName("genre")
			.HasConversion(
				genre => genre.Value.ToString(),
				value => Genre.FromPersistence(value));

		builder
			.Property(movie => movie.ReleaseDate)
			.HasColumnName("release_date")
			.IsRequired()
			.HasColumnType("date")
			.HasConversion(
				releaseDate => releaseDate!.Value,
				value => ReleaseDate.FromPersistence(value));

		builder
			.Property(movie => movie.AverageScore)
			.HasColumnName("average_score")
			.IsRequired()
			.HasPrecision(
				MovieConstants.AverageScore.MaxDigitsPrecision,
				MovieConstants.AverageScore.DecimalPlacesScale)
			.HasConversion(
				averageScore => averageScore.Value,
				value => AverageScore.FromPersistence(value));

		builder
			.Property(movie => movie.ReviewCount)
			.HasColumnName("review_count")
			.IsRequired()
			.HasConversion(
				reviewCount => reviewCount.Value,
				value => ReviewCount.FromPersistence(value));

		builder
			.Property(movie => movie.CreatedAtUtc)
			.HasColumnName("created_at_utc")
			.IsRequired();

		ConfigureComments(builder);
		ConfigureReviews(builder);
	}

	private static void ConfigureComments(EntityTypeBuilder<Movie> builder)
	{
		builder.OwnsMany(
			movie => movie.Comments,
			commentBuilder =>
			{
				commentBuilder.ToTable(
					"comments",
					table =>
					{
						table.HasCheckConstraint(
							"CK_comments_content_not_empty",
							"length(content) > 0");
					});

				commentBuilder.HasKey(comment => comment.Id);

				commentBuilder
					.Property(comment => comment.Id)
					.HasColumnName("comment_id")
					.HasConversion(
						commentId => commentId.Value,
						value => CommentId.FromPersistence(value));

				commentBuilder
					.Property(comment => comment.UserId)
					.HasColumnName("user_id")
					.IsRequired()
					.HasConversion(
						userId => userId.Value,
						value => UserId.FromPersistence(value));

				commentBuilder
					.Property(comment => comment.MovieId)
					.HasColumnName("movie_id")
					.IsRequired()
					.HasConversion(
						movieId => movieId.Value,
						value => MovieId.FromPersistence(value));

				commentBuilder
					.WithOwner()
					.HasForeignKey(comment => comment.MovieId);

				commentBuilder
					.Property(comment => comment.Content)
					.HasColumnName("content")
					.IsRequired()
					.HasMaxLength(MovieConstants.Content.MaxLength)
					.HasConversion(
						content => content.Value,
						value => Content.FromPersistence(value));

				commentBuilder
					.Property(comment => comment.CreatedAtUtc)
					.HasColumnName("created_at_utc")
					.IsRequired();
			});
	}

	private static void ConfigureReviews(EntityTypeBuilder<Movie> builder)
	{
		builder.OwnsMany(
			movie => movie.Reviews,
			reviewBuilder =>
			{
				reviewBuilder.ToTable(
					"reviews",
					table =>
					{
						table.HasCheckConstraint(
							"CK_reviews_score_range",
							$"score >= {MovieConstants.Score.MinScore}"
							+ $" AND score <= {MovieConstants.Score.MaxScore}");
					});

				reviewBuilder
					.HasIndex(review => new { review.MovieId, review.UserId })
					.IsUnique()
					.HasDatabaseName("IX_reviews_movie_id_user_id_unique");

				reviewBuilder.HasKey(review => review.Id);

				reviewBuilder
					.Property(review => review.Id)
					.HasColumnName("review_id")
					.HasConversion(
						reviewId => reviewId.Value,
						value => ReviewId.FromPersistence(value));

				reviewBuilder
					.Property(review => review.UserId)
					.HasColumnName("user_id")
					.IsRequired()
					.HasConversion(
						userId => userId.Value,
						value => UserId.FromPersistence(value));

				reviewBuilder
					.Property(review => review.MovieId)
					.HasColumnName("movie_id")
					.IsRequired()
					.HasConversion(
						movieId => movieId.Value,
						value => MovieId.FromPersistence(value));

				reviewBuilder
					.WithOwner()
					.HasForeignKey(review => review.MovieId);

				reviewBuilder
					.Property(review => review.Score)
					.HasColumnName("score")
					.IsRequired()
					.HasConversion(
						score => score.Value,
						value => Score.FromPersistence(value));

				reviewBuilder
					.Property(review => review.CreatedAtUtc)
					.HasColumnName("created_at_utc")
					.IsRequired();
			});
	}
}
