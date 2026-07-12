using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Application.Movies.Commands.DeleteComment;

internal sealed class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand>
{
	private readonly IMovieWriteRepository _movieWriteRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteCommentCommandHandler(
		IMovieWriteRepository movieWriteRepository,
		IUnitOfWork unitOfWork)
	{
		_movieWriteRepository = movieWriteRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(
		DeleteCommentCommand request,
		CancellationToken cancellationToken)
	{		
		Result<UserId> userIdResult = UserId.Create(request.UserId);
		Result<MovieId> movieIdResult = MovieId.Create(request.MovieId);
		Result<CommentId> commentIdResult = CommentId.Create(request.CommentId);
		Result result = Result.Combine([userIdResult, movieIdResult, commentIdResult]);

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

		movie.DeleteCommentForUser(commentIdResult.Value, userIdResult.Value);
		
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
