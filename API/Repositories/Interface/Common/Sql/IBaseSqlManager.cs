using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Repositories.Interface.Common.Sql
{
    public interface IBaseSqlManager
    {
        #region AsyncOpenSqlConnection

        #region OpenSqlConnectionAsync

        /// <summary>
        /// Asynchronously opens a SQL connection.
        /// </summary>
        /// <returns>An open SQL connection.</returns>
        Task<SqlConnection> OpenSqlConnectionAsync();

        #endregion OpenSqlConnectionAsync

        #region CreateSqlCommandAsync

        /// <summary>
        /// Asynchronously creates a SQL command with the specified parameters and ensures the connection is open.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <param name="queryOrProcedure">The query or stored procedure name.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="sqlTransaction">The SQL sqlTransaction (optional).</param>
        /// <returns>A task representing the asynchronous operation, returning a configured SQL command.</returns>
        Task<SqlCommand> CreateSqlCommandAsync(SqlConnection sqlConnection, string queryOrProcedure, CommandType commandType, SqlTransaction sqlTransaction = null);

        #endregion CreateSqlCommandAsync

        #region CloseSqlConnectionAsync

        /// <summary>
        /// Asynchronously closes a SQL connection.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection to close.</param>
        Task CloseSqlConnectionAsync(SqlConnection sqlConnection);

        #endregion CloseSqlConnectionAsync

        #endregion AsyncOpenSqlConnection

        #region Non AsyncOpenSqlConnection

        #region OpenSqlConnection

        /// <summary>
        /// Asynchronously opens a SQL connection.
        /// </summary>
        /// <returns>An open SQL connection.</returns>
        SqlConnection OpenSqlConnection();

        #endregion OpenSqlConnection

        #region CreateSqlCommand

        /// <summary>
        /// Asynchronously creates a SQL command with the specified parameters and ensures the connection is open.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <param name="queryOrProcedure">The query or stored procedure name.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="sqlTransaction">The SQL sqlTransaction (optional).</param>
        /// <returns>A task representing the asynchronous operation, returning a configured SQL command.</returns>
        SqlCommand CreateSqlCommand(SqlConnection sqlConnection, string queryOrProcedure, CommandType commandType, SqlTransaction sqlTransaction = null);

        #endregion CreateSqlCommand

        #region CloseSqlConnection

        /// <summary>
        /// Asynchronously closes a SQL connection.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection to close.</param>
        void CloseSqlConnection(SqlConnection sqlConnection);

        #endregion CloseSqlConnection

        #endregion Non AsyncOpenSqlConnection

        #region GetMethods

        /// <summary>
        /// Safely retrieves an integer value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The integer value or default if null.</returns>
        Task<int> GetInt32Async(DbDataReader reader, string columnName);

        /// <summary>
        /// Safely retrieves a string value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The string value or empty string if null.</returns>
        Task<string> GetStringAsync(DbDataReader reader, string columnName);

        /// <summary>
        /// Safely retrieves a decimal value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The decimal value or default if null.</returns>
        Task<decimal> GetDecimalAsync(DbDataReader reader, string columnName);

        /// <summary>
        /// Safely retrieves a decimal value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The decimal value or default if null.</returns>
        Task<double> GetDoubleAsync(DbDataReader reader, string columnName);

        /// <summary>
        /// Safely retrieves a DateTime value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The DateTime value or default if null.</returns>
        Task<DateTime> GetDateTimeAsync(DbDataReader reader, string columnName);

        /// <summary>
        /// Safely retrieves a boolean value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The boolean value or default if null.</returns>
        Task<bool> GetBooleanAsync(DbDataReader reader, string columnName);

        #endregion GetMethods
    }
}
