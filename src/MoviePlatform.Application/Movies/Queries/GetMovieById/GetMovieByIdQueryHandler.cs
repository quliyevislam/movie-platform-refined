using MoviePlatform.Domain.Common;
using MoviePlatform.Domain.Movies;
using MoviePlatform.Application.Common.Messaging;
using MoviePlatform.Application.Movies;

namespace MoviePlatform.Application.Movies.Queries.GetMovieById;

internal sealed class GetMovieByIdQueryHandler
	: IQueryHandler<GetMovieByIdQuery, MovieResponse>
{
	private readonly IMovieReadRepository _movieReadRepository;

	public GetMovieByIdQueryHandler(IMovieReadRepository movieReadRepository)
	{
		_movieReadRepository = movieReadRepository;
	}

	public async Task<Result<MovieResponse>> Handle(
		GetMovieByIdQuery request,
		CancellationToken cancellationToken)
	{
		MovieResponse? movieResponse = await _movieReadRepository.GetByIdAsync(
			request.MovieId,
			cancellationToken);

		if (movieResponse is null)
		{
			return Result.Failure<MovieResponse>(MovieErrors.NotFound);
		}

		return Result.Success<MovieResponse>(movieResponse);
	}
}
