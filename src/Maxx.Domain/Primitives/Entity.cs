namespace Maxx.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity(Guid id) => this.Id = id;

    protected Entity()
    {
    }

    public Guid Id { get; private init; }

    public static bool operator ==(Entity? first, Entity? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity? first, Entity? second) =>
        !(first == second);

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != this.GetType())
        {
            return false;
        }

        return other.Id == this.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        if (obj is not Entity entity)
        {
            return false;
        }

        return entity.Id == this.Id;
    }

    public override int GetHashCode() => this.Id.GetHashCode() * 41;
}