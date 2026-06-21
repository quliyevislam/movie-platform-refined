using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Application.Movies.Commands.DeleteMovie;

internal sealed class DeleteMovieCommandHandler : ICommandHandler<DeleteMovieCommand>
{
	private readonly IMovieWriteRepository _movieWriteRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteMovieCommandHandler(
		IMovieWriteRepository movieWriteRepository,
		IUnitOfWork unitOfWork)
	{
		_movieWriteRepository = movieWriteRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(
		DeleteMovieCommand request,
		CancellationToken cancellationToken)
	{
		Result<UserId> userIdResult = UserId.Create(request.UserId);
		Result<MovieId> movieIdResult = MovieId.Create(request.MovieId);
		Result result = Result.Combine([userIdResult, movieIdResult]);

		if (result.IsFailure)
		{
			return Result.Failure(result.Errors);
		}

		Movie? movie = await _movieWriteRepository.GetByIdAsync(
			movieIdResult.Value,
			cancellationToken);

		if (movie is null)
		{
			return Result.Failure(MovieErrors.NotFound);
		}

		if (movie.UserId != userIdResult.Value)
		{
			return Result.Failure(MovieErrors.Forbidden);
		}

		_movieWriteRepository.Remove(movie);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
