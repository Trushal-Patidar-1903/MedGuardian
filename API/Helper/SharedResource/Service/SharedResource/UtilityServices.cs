using DTO.Common.Response;
using Helper.SharedResource.Interface.Utility;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Helper.SharedResource.Service.SharedResource
{
    public class UtilityServices : IUtilityServices
    {
        #region Compare Strings

        /// <summary>
        /// Compares two strings using ordinal comparison.
        /// </summary>
        /// <param name="str1">The first string to compare.</param>
        /// <param name="str2">The second string to compare.</param>
        /// <returns>True if the strings are equal; otherwise, false.</returns>
        /// <example>
        /// bool areEqual = UtilityFunctions.Compare("hello", "hello"); // true
        /// </example>
        public bool Compare(string str1, string str2)
        {
            return string.Compare(str1, str2, StringComparison.Ordinal) == 0;
        }

        #endregion

        #region Create Exception Response Model

        /// <summary>
        /// Creates a standardized error response model using the provided exception.
        /// </summary>
        /// <typeparam name="T">The type of data in the response model.</typeparam>
        /// <param name="ex">The exception to wrap in the response model.</param>
        /// <returns>A structured response model containing error details.</returns>
        /// <example>
        /// var response = UtilityFunctions.CreateExceptionResponseModel<MyType>(ex);
        /// </example>
        public AddEditResponseModel<T> CreateExceptionResponseModel<T>(Exception ex)
        {
            return new AddEditResponseModel<T>
            {
                status = false,
                message = FormatExceptionMessage(ex),
                exception = ex,
            };
        }

        #endregion

        #region Format Exception Message

        /// <summary>
        /// Formats an exception into a detailed string including type, message, source, stack trace, line number, and inner exception.
        /// </summary>
        /// <param name="ex">The exception to format.</param>
        /// <returns>A detailed formatted string describing the exception.</returns>
        /// <example>
        /// try
        /// {
        ///     // code that throws
        /// }
        /// catch (Exception ex)
        /// {
        ///     string message = UtilityFunctions.FormatExceptionMessage(ex);
        /// }
        /// </example>
        public string FormatExceptionMessage(Exception ex)
        {
            if (ex == null)
                return "Exception object is null.";

            var lineNumber = ex.StackTrace?.Split('\n')
                .LastOrDefault(line => line.Contains(":line "))
                ?.Trim();

            var formattedMessage = new System.Text.StringBuilder();
            formattedMessage.AppendLine($"Exception Type: {ex.GetType().Name}");
            formattedMessage.AppendLine($"Message: {ex.Message}");
            formattedMessage.AppendLine($"Source: {ex.Source}");
            formattedMessage.AppendLine($"Stack Trace: {ex.StackTrace}");
            formattedMessage.AppendLine($"Occurred at (Line number): {lineNumber}");

            if (ex.InnerException != null)
            {
                formattedMessage.AppendLine("--- Inner Exception ---");
                formattedMessage.AppendLine($"Exception Type: {ex.InnerException.GetType().Name}");
                formattedMessage.AppendLine($"Message: {ex.InnerException.Message}");
                formattedMessage.AppendLine($"Source: {ex.InnerException.Source}");
                formattedMessage.AppendLine($"Stack Trace: {ex.InnerException.StackTrace}");
            }

            return formattedMessage.ToString();
        }

        #endregion

        #region Convert List to DataTable

        /// <summary>
        /// Converts a list of complex objects into a DataTable by mapping properties to columns.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="data">The list of data to convert.</param>
        /// <returns>A DataTable representation of the list.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input list is null.</exception>
        /// <example>
        /// var table = UtilityFunctions.ConvertListToDataTable(new List<Person> { new Person { Name = "John" } });
        /// </example>
        public DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Input list cannot be null.");

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable CreateEmptyTable<T>() where T : new()
        {
            return ConvertListToDataTable(new List<T>());
        }
        #endregion

        #region Convert DataTable to List<T>

        /// <summary>
        /// Converts a DataTable to a list of strongly-typed objects using reflection.
        /// </summary>
        /// <typeparam name="T">The type of object to convert to.</typeparam>
        /// <param name="table">The DataTable to convert.</param>
        /// <returns>A list of objects created from the DataTable rows.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the DataTable is null.</exception>
        /// <example>
        /// DataTable dt = GetDataTable();
        /// List<Person> people = dt.ConvertDataTableToList<Person>();
        /// </example>
        public List<T> ConvertDataTableToList<T>(DataTable table) where T : new()
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table), "Input DataTable cannot be null.");

            var list = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (DataRow row in table.Rows)
            {
                var obj = new T();

                foreach (var prop in properties)
                {
                    if (table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    {
                        try
                        {
                            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var safeValue = Convert.ChangeType(row[prop.Name], propType);
                            prop.SetValue(obj, safeValue, null);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException($"Failed to map column '{prop.Name}' to property '{prop.Name}'.", ex);
                        }
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        #endregion

        #region Create List DataTable (Primitive Types)

        /// <summary>
        /// Efficiently creates a DataTable from a list of any primitive or reference type (e.g., int, string, DateTime).
        /// </summary>
        /// <typeparam name="T">The type of the items in the list.</typeparam>
        /// <param name="items">The list of items to convert to a DataTable.</param>
        /// <param name="columnName">Optional column name for the single column. Defaults to "Value".</param>
        /// <returns>A DataTable with a single column containing the items from the list.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list is null or empty.</exception>
        /// <example>
        /// var intTable = UtilityFunctions.CreateListDataTable(new List<int> { 1, 2, 3 });
        /// var stringTable = UtilityFunctions.CreateListDataTable(new List<string> { "A", "B" }, "Names");
        /// </example>
        public DataTable CreateListDataTable<T>(List<T> items, string columnName = "Value")
        {
            if (items == null || items.Count == 0)
                throw new ArgumentNullException(nameof(items), "The input list cannot be null or empty.");

            var table = new DataTable();

            try
            {
                table.Columns.Add(columnName, typeof(T));

                foreach (var item in items)
                {
                    table.Rows.Add(item == null ? DBNull.Value : item);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to convert list of type '{typeof(T)}' to DataTable. See inner exception for details.", ex);
            }

            return table;
        }

        #endregion

        #region ToDbValue - If value is null then return DBNull.Value

        /// <summary>
        /// Converts a base C# value to `DBNull.Value` if it represents a default or empty value.
        /// </summary>
        public object ToDbValue(object value)
        {
            if (value == null)
                return DBNull.Value;

            return value switch
            {
                string str when string.IsNullOrWhiteSpace(str) => DBNull.Value,
                DateTime dt when dt == DateTime.MinValue || dt.Year == 1 => DBNull.Value,
                int i when i == 0 => DBNull.Value,
                long l when l == 0 => DBNull.Value,
                double d when d == 0 => DBNull.Value,
                _ => value
            };
        }

        #endregion ToDbValue - If value is null then return DBNull.Value

        #region SqlNow - Currect Server DateTime

        /// <summary>
        /// Returns current DateTime for SQL parameter.
        /// </summary>
        public object SqlNow() => DateTime.Now;

        #endregion SqlNow - Currect Server DateTime
    }
}
