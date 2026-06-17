using MediatR;

namespace MoviePlatform.Domain.Common;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredAt { get; }
}
