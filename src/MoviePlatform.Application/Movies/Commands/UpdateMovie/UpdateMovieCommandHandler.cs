using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Application.Movies.Commands.UpdateMovie;

internal sealed class UpdateMovieCommandHandler : ICommandHandler<UpdateMovieCommand, Guid>
{
	private readonly IMovieWriteRepository _movieWriteRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly TimeProvider _timeProvider;

	public UpdateMovieCommandHandler(
		IMovieWriteRepository movieWriteRepository,
		IUnitOfWork unitOfWork,
		TimeProvider timeProvider)
	{
		_movieWriteRepository = movieWriteRepository;
		_unitOfWork = unitOfWork;
		_timeProvider = timeProvider;
	}

	public async Task<Result<Guid>> Handle(
		UpdateMovieCommand request,
		CancellationToken cancellationToken)
	{
		Result<UserId> userIdResult = UserId.Create(request.UserId);
		Result<MovieId> movieIdResult = MovieId.Create(request.MovieId);
		Result<Title> titleResult = Title.Create(request.Title);
		Result<Description> descriptionResult = Description.Create(request.Description);
		Result<Genre> genreResult = Genre.Create(request.Genre);
		Result<ReleaseDate> releaseDateResult = ReleaseDate.Create(
			request.ReleaseDate,
			_timeProvider.GetUtcNow());
		Result result = Result.Combine([
			userIdResult,
			movieIdResult,
			titleResult,
			descriptionResult,
			genreResult,
			releaseDateResult]);

		if (result.IsFailure)
		{
			return Result.Failure<Guid>(result.Errors);
		}

		Movie? movie = await _movieWriteRepository.GetByIdAsync(
			movieIdResult.Value,
			cancellationToken);

		if (movie is null)
		{
			return Result.Failure<Guid>(MovieErrors.NotFound);
		}

		if (movie.UserId != userIdResult.Value)
		{
			return Result.Failure<Guid>(MovieErrors.Forbidden);
		}

		movie.Update(
			titleResult.Value,
			descriptionResult.Value,
			genreResult.Value,
			releaseDateResult.Value);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success<Guid>(movie.Id.Value);
	}
}
