using MediatR;
using MoviePlatform.Domain.Common;

namespace MoviePlatform.Application.Common.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
	: IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;
