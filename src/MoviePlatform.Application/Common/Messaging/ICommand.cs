using MediatR;
using MoviePlatform.Domain.Common;

namespace MoviePlatform.Application.Common.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
