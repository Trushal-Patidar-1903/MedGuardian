using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories.Interface.Common.Sql;
using System.Data;
using System.Data.Common;

namespace Repositories.Repository.Common.Sql
{
    public class BaseSqlManager : IBaseSqlManager
    {
        private readonly IConfiguration _iConfiguration;
        public BaseSqlManager(IConfiguration _iConfiguration)
        {
            this._iConfiguration = _iConfiguration;
        }

        #region AsyncOpenSqlConnection

        #region OpenSqlConnectionAsync

        /// <summary>
        /// Asynchronously opens a SQL connection.
        /// </summary>
        /// <returns>An open SQL connection.</returns>
        public async Task<SqlConnection> OpenSqlConnectionAsync()
        {
            SqlConnection sqlConnection = new(_iConfiguration.GetConnectionString(Helper.Common.Strings.MedGuardianConnString));
            if (sqlConnection.State == ConnectionState.Closed)
            {
                await sqlConnection.OpenAsync();
            }
            return sqlConnection;
        }

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
        public async Task<SqlCommand> CreateSqlCommandAsync(
            SqlConnection sqlConnection,
            string queryOrProcedure,
            CommandType commandType,
            SqlTransaction sqlTransaction = null)
        {
            // Ensure the connection is open asynchronously
            if (sqlConnection.State != ConnectionState.Open)
            {
                await sqlConnection.OpenAsync();
            }

            // Configure the SQL command
            SqlCommand sqlCommand = new(queryOrProcedure, sqlConnection)
            {
                CommandType = commandType,
                CommandTimeout = 0, // Set a reasonable timeout for better resource management
                Transaction = sqlTransaction
            };

            return sqlCommand;
        }

        #endregion CreateSqlCommandAsync

        #region CloseSqlConnectionAsync

        /// <summary>
        /// Asynchronously closes a SQL connection.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection to close.</param>
        public async Task CloseSqlConnectionAsync(SqlConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                await sqlConnection.CloseAsync();
            }
        }

        #endregion CloseSqlConnectionAsync

        #endregion AsyncOpenSqlConnection

        #region Non AsyncOpenSqlConnection

        #region OpenSqlConnection

        /// <summary>
        /// Asynchronously opens a SQL connection.
        /// </summary>
        /// <returns>An open SQL connection.</returns>
        public SqlConnection OpenSqlConnection()
        {
            SqlConnection sqlConnection = new(_iConfiguration.GetConnectionString(Helper.Common.Strings.MedGuardianConnString));
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            return sqlConnection;
        }

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
        public SqlCommand CreateSqlCommand(
            SqlConnection sqlConnection,
            string queryOrProcedure,
            CommandType commandType,
            SqlTransaction sqlTransaction = null)
        {
            // Ensure the connection is open asynchronously
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            // Configure the SQL command
            SqlCommand sqlCommand = new(queryOrProcedure, sqlConnection)
            {
                CommandType = commandType,
                CommandTimeout = 0, // Set a reasonable timeout for better resource management
                Transaction = sqlTransaction
            };

            return sqlCommand;
        }

        #endregion CreateSqlCommand

        #region CloseSqlConnection

        /// <summary>
        /// Asynchronously closes a SQL connection.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection to close.</param>
        public void CloseSqlConnection(SqlConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        #endregion CloseSqlConnection

        #endregion Non AsyncOpenSqlConnection

        #region GetMethods

        /// <summary>
        /// Safely retrieves an integer value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The integer value or default if null.</returns>
        public async Task<int> GetInt32Async(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return !await reader.IsDBNullAsync(ordinal) ? reader.GetInt32(ordinal) : default;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Safely retrieves a string value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The string value or empty string if null.</returns>
        public async Task<string> GetStringAsync(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return !await reader.IsDBNullAsync(ordinal) ? reader.GetString(ordinal) : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Safely retrieves a decimal value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The decimal value or default if null.</returns>
        public async Task<decimal> GetDecimalAsync(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return !await reader.IsDBNullAsync(ordinal) ? reader.GetDecimal(ordinal) : default;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Safely retrieves a decimal value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The decimal value or default if null.</returns>
        public async Task<double> GetDoubleAsync(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return !await reader.IsDBNullAsync(ordinal) ? reader.GetDouble(ordinal) : default;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Safely retrieves a DateTime value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The DateTime value or default if null.</returns>
        public async Task<DateTime> GetDateTimeAsync(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return !await reader.IsDBNullAsync(ordinal) ? reader.GetDateTime(ordinal) : default;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Safely retrieves a boolean value from the specified column.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The boolean value or default if null.</returns>
        public async Task<bool> GetBooleanAsync(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return !await reader.IsDBNullAsync(ordinal) && reader.GetBoolean(ordinal);
            }
            catch
            {
                return default;
            }
        }

        #endregion GetMethods
    }
}