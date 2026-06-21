using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetMovieByIdAndUserId;

internal sealed class GetMovieByIdAndUserIdQueryHandler
	: IQueryHandler<GetMovieByIdAndUserIdQuery, MovieResponse>
{
	private readonly IMovieReadRepository _movieReadRepository;

	public GetMovieByIdAndUserIdQueryHandler(IMovieReadRepository movieReadRepository)
	{
		_movieReadRepository = movieReadRepository;
	}

	public async Task<Result<MovieResponse>> Handle(
		GetMovieByIdAndUserIdQuery request,
		CancellationToken cancellationToken)
	{
		MovieResponse? movieResponse = await _movieReadRepository.GetByIdAndUserIdAsync(
			request.MovieId,
			request.UserId,
			cancellationToken);

		if (movieResponse is null)
		{
			return Result.Failure<MovieResponse>(MovieErrors.NotFound);
		}

		return Result.Success<MovieResponse>(movieResponse);
	}
}
