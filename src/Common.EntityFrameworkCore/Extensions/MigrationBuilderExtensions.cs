using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Common.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public static class MigrationBuilderExtensions
    {
        /// <summary>
        /// Adds a required column to the migration builder with an intermediary SQL update statement to provide an initial value
        /// for the non-nullable field.
        /// Either supply a simple initial value with <paramref name="sqlUpdateInitialValue"/> or the full update script <paramref name="sqlUpdateScript"/>.
        /// This function will create a nullable column first, execute a SQL update statement to supply an initial value for all rows,
        /// then alter the column to be non-nullable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="migrationBuilder"></param>
        /// <param name="name"></param>
        /// <param name="table"></param>
        /// <param name="sqlUpdateInitialValue"></param>
        /// <param name="sqlUpdateScript"></param>
        /// <param name="type"></param>
        /// <param name="unicode"></param>
        /// <param name="maxLength"></param>
        /// <param name="rowVersion"></param>
        /// <param name="schema"></param>
        /// <param name="fixedLength"></param>
        /// <returns></returns>
        public static OperationBuilder<AddColumnOperation> AddRequiredColumnWithData<T>(this MigrationBuilder migrationBuilder,
            string name, string table, string sqlUpdateInitialValue = null, string sqlUpdateScript = null,
            string type = null, bool? unicode = null, int? maxLength = null, bool rowVersion = false, string schema = null,
            bool? fixedLength = null)
        {
            Guard.IsNotNull(migrationBuilder, nameof(migrationBuilder));
            Guard.IsNotNull(name, nameof(name));
            Guard.IsNotNull(table, nameof(table));

            if (string.IsNullOrWhiteSpace(sqlUpdateInitialValue) && string.IsNullOrWhiteSpace(sqlUpdateScript))
                throw new ArgumentNullException($"{nameof(sqlUpdateInitialValue)} / {nameof(sqlUpdateScript)}", $"Either {nameof(sqlUpdateInitialValue)} parameter or {nameof(sqlUpdateScript)} parameter are required.");

            var operation = migrationBuilder.AddColumn<T>(
                name: name,
                table: table,
                nullable: true, // null by default so no SQL table errors
                type: type,
                unicode: unicode,
                maxLength: maxLength,
                rowVersion: rowVersion,
                schema: schema,
                fixedLength: fixedLength);

            // use provided custom update script by default
            string sql = sqlUpdateScript;

            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = $@"UPDATE [{table}]
                         SET [{name}] = {sqlUpdateInitialValue};";
            }

            // run SQL update statement to update this new nullable column
            migrationBuilder.Sql(sql);

            // set column to not-nullable
            migrationBuilder.AlterColumn<T>(
                name: name,
                table: table,
                nullable: false);

            return operation;
        }

        /// <summary>
        /// Alter an existing primary key column to or from Identity Specification.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="migrationBuilder"></param>
        /// <param name="table">Table name.</param>
        /// <param name="tableColumns">All columns on the table. This is required to not have any data loss.</param>
        /// <param name="setIdentity">Whether or not Identity Specification should be enabled.</param>
        /// <param name="column">Primary Key column name. Defaults to "Id".</param>
        /// <param name="type">Primary Key column type. By default, based on <typeparamref name="T"/> being <see cref="int"/> or <see cref="long"/>, "int" or "bigint" is used.</param>
        /// <param name="pkName">Primary Key name. Defaults to "PK_[table]"</param>
        /// <param name="onDropConstraints">Call back prior to perfoming the PK altering to drop constraints on other tables.</param>
        /// <param name="onAddConstraints">Call back after perfoming the PK altering to add back constraints on other tables.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static MigrationBuilder AlterPrimaryKeyColumn<T>(
            this MigrationBuilder migrationBuilder,
            string table,
            HashSet<string> tableColumns,
            bool setIdentity = true,
            string column = "id",
            string type = null,
            string pkName = null,
            Action<MigrationBuilder> onDropConstraints = null,
            Action<MigrationBuilder> onAddConstraints = null)
        {
            Guard.IsNotNull(tableColumns, nameof(tableColumns));

            if (string.IsNullOrWhiteSpace(column))
                column = "id";

            if (string.IsNullOrWhiteSpace(type))
                type = typeof(T) == typeof(int) ? "int" : typeof(T) == typeof(long) ? "bigint" : throw new InvalidOperationException("Type could not be inferred on alter column attempt with PK.");

            if (string.IsNullOrWhiteSpace(pkName))
                pkName = $"PK_{table}";

            string tempTable = $"t_{table}";

            migrationBuilder.Sql(@$"
                IF OBJECT_ID(N'dbo.{tempTable}', N'U') IS NOT NULL  
                   DROP TABLE [dbo].[{tempTable}];  
                GO");

            migrationBuilder.Sql($"SELECT * INTO [{tempTable}] FROM [{table}]");

            onDropConstraints?.Invoke(migrationBuilder);

            migrationBuilder.Sql($"DELETE FROM [{table}]");

            // drop PK to remove constraint
            migrationBuilder.DropPrimaryKey(
               name: pkName,
               table: table);

            // drop/set ID to identity (needs to be done manually because the migration do not know about restrictions)
            migrationBuilder.DropColumn(
                name: column,
                table: table);

            // add back PK column
            var altercolumnOperation = migrationBuilder.AddColumn<T>(
                name: column,
                table: table,
                type: type,
                nullable: false);

            if (setIdentity)
                altercolumnOperation.Annotation("SqlServer:Identity", "1, 1");

            // set PK back
            migrationBuilder.AddPrimaryKey(
                name: pkName,
                table: table,
                column: column);

            var dataColumnsForSqlInsert = string.Join(",", tableColumns.Select(c => $"[{c}]"));

            if (setIdentity)
                migrationBuilder.Sql($"SET IDENTITY_INSERT [{table}] ON");

            // insert data back from temp table
            migrationBuilder.Sql(@$"
                INSERT INTO [dbo].[{table}] ({dataColumnsForSqlInsert})
                SELECT {dataColumnsForSqlInsert}
                FROM [{tempTable}]");

            if (setIdentity)
                migrationBuilder.Sql($"SET IDENTITY_INSERT [{table}] OFF");

            migrationBuilder.Sql($"DROP TABLE [{tempTable}]");

            onAddConstraints?.Invoke(migrationBuilder);

            return migrationBuilder;
        }
    }
}
