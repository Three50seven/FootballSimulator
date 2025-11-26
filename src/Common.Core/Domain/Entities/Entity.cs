namespace Common.Core.Domain
{
    /// <summary>
    /// Base entity implentation. Includes standard int Id value and Equals overrides.
    /// </summary>
    public abstract class Entity : Entity<int>
    {
        protected Entity()
        {
        }

        protected Entity(int id) 
            : base(id)
        {
        }
    }


    /// <summary>
    /// Base entity implentation. Includes standard Id value and Equals overrides.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class Entity<TId> : IEquatable<Entity<TId>>, IEntity<TId>
    {
        protected Entity() 
        { 
            Id = default!;
        }

        protected Entity(TId id) : this()
        {
            if (object.Equals(id, default(TId)))
                throw new ArgumentException($"The Identifier of type {typeof(TId)} cannot be the type's default value.");

            Id = id;
        }

        public TId Id { get; protected set; }

        public virtual bool IsNew => object.Equals(Id, default(TId)) || (typeof(TId) == typeof(int) && Convert.ToInt32(Id) <= 0);

        public override bool Equals(object? obj)
        {
            if (obj is Entity<TId> entity)
                return Equals(entity);

            return base.Equals(obj);
        }


        public override int GetHashCode() => Id?.GetHashCode() ?? 0;

        public virtual bool Equals(Entity<TId>? other)
        {
            if (other is null)
                return false;

            // If Id itself is nullable, use ?.Equals with null-coalescing
            return Id?.Equals(other.Id) ?? false;
        }

        public static bool operator ==(Entity<TId> x, Entity<TId> y)
        {
            if (Equals(null, x) && Equals(null, y))
                return true;

            if (Equals(null, x))
                return false;

            return x.Equals(y);
        }

        public static bool operator !=(Entity<TId> x, Entity<TId> y)
        {
            return !(x == y);
        }
    }
}
