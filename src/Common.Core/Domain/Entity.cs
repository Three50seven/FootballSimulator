namespace Common.Core.Domain
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>, IEntity<TId>, IEntity
    {
        public TId Id { get; protected set; } = default!;

        public virtual bool IsNew => object.Equals(Id, default(TId)) || (typeof(TId) == typeof(int) && Convert.ToInt32(Id) <= 0);

        protected Entity()
        {
        }

        protected Entity(TId id)
            : this()
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException($"The Identifier of type {typeof(TId)} cannot be the type's default value.");
            }

            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Entity<TId> entity)
            {
                return Equals(entity);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id is not null ? Id.GetHashCode() : 0;
        }

        public virtual bool Equals(Entity<TId>? other)
        {
            if (other == null)
            {
                return false;
            }

            if (Id is null || other.Id is null)
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TId>? x, Entity<TId>? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(Entity<TId>? x, Entity<TId>? y)
        {
            return !(x == y);
        }
    }
}
