using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Domain.Movies.Entities;
using MoviePlatform.Domain.Movies.ValueObjects;
using MoviePlatform.Domain.Users.ValueObjects;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Application.Movies.Commands.AddComment;

internal sealed class AddCommentCommandHandler : ICommandHandler<AddCommentCommand, Guid>
{
	private readonly IMovieWriteRepository _movieWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
	private readonly TimeProvider _timeProvider;

	public AddCommentCommandHandler(
		IMovieWriteRepository movieWriteRepository,
		IUnitOfWork unitOfWork,
		TimeProvider timeProvider)
	{
		_movieWriteRepository = movieWriteRepository;
		_unitOfWork = unitOfWork;
		_timeProvider = timeProvider;
	}

	public async Task<Result<Guid>> Handle(
		AddCommentCommand request,
		CancellationToken cancellationToken)
	{
		Result<UserId> userIdResult = UserId.Create(request.UserId);
		Result<MovieId> movieIdResult = MovieId.Create(request.MovieId);
		Result<Content> contentResult = Content.Create(request.Content);
		Result result = Result.Combine([userIdResult, movieIdResult, contentResult]);

		if (result.IsFailure)
		{
			return Result.Failure<Guid>(result.Errors);
		}

		Movie? movie = await _movieWriteRepository.GetByIdAsync(movieIdResult.Value, cancellationToken);

		if (movie is null)
		{
			return Result.Failure<Guid>(MovieErrors.NotFound);
		}

		Comment comment = Comment.Create(
			userIdResult.Value,
			contentResult.Value,
			_timeProvider.GetUtcNow());

		movie.AddComment(comment);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success<Guid>(comment.Id.Value);
	}
}
