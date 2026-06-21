using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Application.Movies.Commands.CreateMovie;

internal sealed class CreateMovieCommandHandler : ICommandHandler<CreateMovieCommand, Guid>
{
	private readonly IMovieWriteRepository _movieWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
	private readonly TimeProvider _timeProvider;

	public CreateMovieCommandHandler(
		IMovieWriteRepository movieWriteRepository,
		IUnitOfWork unitOfWork,
		TimeProvider timeProvider)
	{
		_movieWriteRepository = movieWriteRepository;
		_unitOfWork = unitOfWork;
		_timeProvider = timeProvider;
	}

	public async Task<Result<Guid>> Handle(
		CreateMovieCommand request,
		CancellationToken cancellationToken)
	{
		Result<UserId> userIdResult = UserId.Create(request.UserId);
		Result<Title> titleResult = Title.Create(request.Title);
		Result<Description> descriptionResult = Description.Create(request.Description);
		Result<Genre> genreResult = Genre.Create(request.Genre);
		Result<ReleaseDate> releaseDateResult = ReleaseDate.Create(
			request.ReleaseDate,
			_timeProvider.GetUtcNow());
		Result result = Result.Combine([
			userIdResult,
			titleResult,
			descriptionResult,
			genreResult,
			releaseDateResult]);

		if (result.IsFailure)
		{
			return Result.Failure<Guid>(result.Errors);
		}

		Movie movie = Movie.Create(
			userIdResult.Value,
			titleResult.Value,
			descriptionResult.Value,
			genreResult.Value,
			releaseDateResult.Value,
			_timeProvider.GetUtcNow());

		_movieWriteRepository.Add(movie);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success<Guid>(movie.Id.Value);
	}
}
