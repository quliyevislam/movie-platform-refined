namespace MoviePlatform.Domain.Common;

public abstract record DomainEvent : IDomainEvent
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
