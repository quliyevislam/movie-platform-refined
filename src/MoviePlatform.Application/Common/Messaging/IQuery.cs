using MoviePlatform.Domain.Common;
using MediatR;

namespace MoviePlatform.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
