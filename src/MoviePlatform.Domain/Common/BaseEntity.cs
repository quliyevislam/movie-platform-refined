namespace MoviePlatform.Domain.Common;

public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
{
	public TId Id { get; protected set; } = default!;

	public bool Equals(BaseEntity<TId>? other)
	{
		if (other is null || Id is null)
		{
			return false;
		}

        if (ReferenceEquals(this, other))
		{
			return true;
		}

		if (other.GetType() != GetType())
		{
			return false;
		}

		return Id.Equals(other.Id);
	}

	public override bool Equals(object? obj) => obj is BaseEntity<TId> other && Equals(other);

	public override int GetHashCode() => HashCode.Combine(GetType(), Id);

	public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
	{
		if (left is null && right is null)
		{
			return true;
		}

		if (left is null || right is null)
		{
			return false;
		}

		return left.Equals(right);
	}

	public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right) => !(left == right);
}
