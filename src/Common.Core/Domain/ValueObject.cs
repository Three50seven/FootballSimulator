using System.Reflection;

namespace Common.Core.Domain
{
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        private const int HASH_START_VALUE = 17;

        private const int HASH_MULTIPLIER = 59;

        public override bool Equals(object? obj)
        {
            if (obj is not T other)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            IEnumerable<FieldInfo> fields = GetFields();
            int num = 17;
            foreach (FieldInfo item in fields)
            {
                object? value = item.GetValue(this);
                if (value != null)
                {
                    num = num * 59 + value.GetHashCode();
                }
            }

            return num;
        }

        public virtual bool Equals(T? other)
        {
            if (other is null)
            {
                return false;
            }

            Type type = GetType();
            Type type2 = other.GetType();
            if (type != type2)
            {
                return false;
            }

            IEnumerable<FieldInfo> fields = GetFields();
            foreach (FieldInfo item in fields)
            {
                object? value = item.GetValue(other);
                object? value2 = item.GetValue(this);
                if (value == null)
                {
                    if (value2 != null)
                    {
                        return false;
                    }
                }
                else if (!value.Equals(value2))
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            Type? type = GetType();
            List<FieldInfo> list = new List<FieldInfo>();
            while (type != null && type != typeof(object))
            {
                list.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
                type = type.BaseType;
            }

            return list;
        }

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
        {
            if (object.Equals(null, x) && object.Equals(null, y))
            {
                return true;
            }

            if (object.Equals(null, x))
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(ValueObject<T> x, ValueObject<T> y)
        {
            return !(x == y);
        }
    }
}
