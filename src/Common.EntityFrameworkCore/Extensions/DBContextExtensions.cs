using Common.Core.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Reflection;

namespace Common.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Ensures <paramref name="item"/> entry is attached to the <paramref name="context"/> and sets the state to <see cref="EntityState.Modified"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="context"></param>
        /// <param name="item"></param>
        public static void SetAsModified<T, K>(this DbContext context, T item) where T : class, IEntity<K>
        {
            AttachAndSetState<T, K>(context, item, EntityState.Modified);
        }

        /// <summary>
        /// Ensures <paramref name="item"/> entry is attached to the <paramref name="context"/> and sets the state to <see cref="EntityState.Modified"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="item"></param>
        public static void SetAsModified<T>(this DbContext context, T item) where T : class, IDomainEntity
        {
            SetAsModified<T, int>(context, item);
        }

        /// <summary>
        /// Ensures <paramref name="item"/> entry is attached to the <paramref name="context"/> and sets the state to <paramref name="state"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="context"></param>
        /// <param name="item"></param>
        /// <param name="state"></param>
        public static void AttachAndSetState<T, K>(this DbContext context, T item, EntityState state) where T : class, IEntity<K>
        {
            var entry = GetEntry<T, K>(context, item);
            AttachEntry<T, K>(context, entry, item);
            entry.State = state;
        }

        /// <summary>
        /// Ensures all <paramref name="items"/> entries are attached to the <paramref name="context"/> and their state is set to <paramref name="state"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="context"></param>
        /// <param name="items"></param>
        /// <param name="state"></param>
        public static void AttachAndSetState<T, K>(this DbContext context, IEnumerable<T> items, EntityState state) where T : class, IEntity<K>
        {
            foreach (var item in items)
            {
                AttachAndSetState<T, K>(context, item, state);
            }
        }

        /// <summary>
        /// Set the <paramref name="item"/> entry state on the <paramref name="context"/> to <paramref name="state"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="context"></param>
        /// <param name="item"></param>
        /// <param name="state"></param>
        public static void SetState<T, K>(this DbContext context, T item, EntityState state) where T : class, IEntity<K>
        {
            var entry = GetEntry<T, K>(context, item);
            entry.State = state;
        }

        /// <summary>
        /// Ensures <paramref name="item"/> entry is attached to the <paramref name="context"/> and sets the state to <see cref="EntityState.Unchanged"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="context"></param>
        /// <param name="item"></param>
        public static void RevertEntityChange<T, K>(this DbContext context, T item) where T : class, IEntity<K>
        {
            AttachAndSetState<T, K>(context, item, EntityState.Unchanged);
        }

        /// <summary>
        /// Ensures all <paramref name="items"/> entries are attached to the <paramref name="context"/> and set the states to <see cref="EntityState.Unchanged"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="context"></param>
        /// <param name="items"></param>
        public static void RevertEntityChange<T, K>(this DbContext context, IEnumerable<T> items) where T : class, IEntity<K>
        {
            AttachAndSetState<T, K>(context, items, EntityState.Unchanged);
        }

        /// <summary>
        /// Get the <see cref="EntityEntry{TEntity}"/> on <paramref name="context"/> for the given <paramref name="item"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="context"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static EntityEntry<T> GetEntry<T, K>(this DbContext context, T item) where T : class, IEntity<K>
        {
            return context.Entry(item);
        }

        /// <summary>
        /// Check the <paramref name="entry"/> and attaches <paramref name="item"/> to <paramref name="context"/> if the entry is <see cref="EntityState.Detached"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="context"></param>
        /// <param name="entry"></param>
        /// <param name="item"></param>
        public static void AttachEntry<T, K>(this DbContext context, EntityEntry<T> entry, T item) where T : class, IEntity<K>
        {
            if (entry.State == EntityState.Detached)
                context.Set<T>().Attach(item);
        }

        /// <summary>
        /// Provides simple return type for executing raw SQL on the context. Calls ExecuteSqlRaw.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "SQL passed into this method will be treated as SQL passed into RelationalDatabaseFacadeExtensions.ExecuteSqlRaw")]
        public static T ExecuteScalar<T>(this DbContext context, string sql, params object[] parameters)
        {
            var resultParameter = new SqlParameter("@result", typeof(T) == typeof(int) ? SqlDbType.Int : SqlDbType.VarChar)
            {
                Size = 2000,
                Direction = ParameterDirection.Output
            };

            var paramsList = new List<object>();
            if (parameters != null)
                paramsList.AddRange(parameters);

            paramsList.Add(resultParameter);

            context.Database.ExecuteSqlRaw($"SET @result = ({sql});", [.. paramsList]);
            return (T)resultParameter.Value;
        }

        /// <summary>
        /// Provides simple return type for executing raw SQL on the context. Calls ExecuteSqlRawAsync.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "SQL passed into this method will be treated as SQL passed into RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync")]
        public static async Task<T> ExecuteScalarAsync<T>(this DbContext context, string sql, params object[] parameters)
        {
            var resultParameter = new SqlParameter("@result", typeof(T) == typeof(int) ? SqlDbType.Int : SqlDbType.VarChar)
            {
                Size = 2000,
                Direction = ParameterDirection.Output
            };

            var paramsList = new List<object>();
            if (parameters != null)
                paramsList.AddRange(parameters);

            paramsList.Add(resultParameter);

            await context.Database.ExecuteSqlRawAsync($"SET @result = ({sql});", [.. paramsList]);
            return (T)resultParameter.Value;
        }

        /// <summary>
        /// Get the queryable set from the dbcontext based on the supplied type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IQueryable<object>? Set(this DbContext context, Type type)
        {
            // get "set" method from dbcontext
            MethodInfo? method = typeof(DbContext).GetMethod(nameof(DbContext.Set), []);

            if (method == null)
                return null;

            // set the generic type on method to the dynamic type
            method = method.MakeGenericMethod(type);

            return method.Invoke(context, null) as IQueryable<object>;
        }
    }
}
