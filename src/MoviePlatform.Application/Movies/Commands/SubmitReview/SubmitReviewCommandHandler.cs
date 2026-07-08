using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Domain.Movies.Entities;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Application.Movies.Commands.SubmitReview;

internal sealed class SubmitReviewCommandHandler : ICommandHandler<SubmitReviewCommand, Guid>
{
	private readonly IMovieWriteRepository _movieWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
	private readonly TimeProvider _timeProvider;

	public SubmitReviewCommandHandler(
		IMovieWriteRepository movieWriteRepository,
		IUnitOfWork unitOfWork,
		TimeProvider timeProvider)
	{
		_movieWriteRepository = movieWriteRepository;
		_unitOfWork = unitOfWork;
		_timeProvider = timeProvider;
	}

	public async Task<Result<Guid>> Handle(
		SubmitReviewCommand request,
		CancellationToken cancellationToken)
	{
		Result<UserId> userIdResult = UserId.Create(request.UserId);
		Result<MovieId> movieIdResult = MovieId.Create(request.MovieId);
		Result<Score> scoreResult = Score.Create(request.Score);
		Result result = Result.Combine([userIdResult, movieIdResult, scoreResult]);

		if (result.IsFailure)
		{
			return Result.Failure<Guid>(result.Errors);
		}

		Movie? movie = await _movieWriteRepository.GetByIdAsync(movieIdResult.Value, cancellationToken);

		if (movie is null)
		{
			return Result.Failure<Guid>(MovieErrors.NotFound);
		}

		Review review = Review.Create(
			userIdResult.Value,
			scoreResult.Value,
			_timeProvider.GetUtcNow());

		movie.SubmitReview(review);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success<Guid>(review.Id.Value);
	}
}
