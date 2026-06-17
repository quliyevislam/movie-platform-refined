namespace MoviePlatform.Domain.Common;

public abstract class AggregateRoot<TId> : BaseEntity<TId> where TId : struct
{
	private readonly List<IDomainEvent> _domainEvents = [];
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	protected AggregateRoot(TId id) : base(id) { }

	protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
	public void ClearDomainEvents() => _domainEvents.Clear();
}
