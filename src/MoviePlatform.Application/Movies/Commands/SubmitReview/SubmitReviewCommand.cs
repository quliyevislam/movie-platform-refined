using MoviePlatform.Application.Common.Messaging;

namespace MoviePlatform.Application.Movies.Commands.SubmitReview;

public record SubmitReviewCommand(Guid UserId, Guid MovieId, int Score) : ICommand<Guid>;
