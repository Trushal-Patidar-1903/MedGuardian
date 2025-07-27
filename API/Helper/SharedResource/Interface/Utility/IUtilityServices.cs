using DTO.Common.Response;
using System.Data;

namespace Helper.SharedResource.Interface.Utility
{
    public interface IUtilityServices
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
        bool Compare(string str1, string str2);

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
        AddEditResponseModel<T> CreateExceptionResponseModel<T>(Exception ex);

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
        DataTable ConvertListToDataTable<T>(IList<T> data);

        DataTable CreateEmptyTable<T>() where T : new();

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
        List<T> ConvertDataTableToList<T>(DataTable table) where T : new();

        #endregion

        #region Create List of Any Datatypes to DataTable (Primitive Types)

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
        DataTable CreateListDataTable<T>(List<T> items, string columnName = "Value");

        #endregion

        #region ToDbValue - If value is null then return DBNull.Value

        /// <summary>
        /// Converts a base C# value to `DBNull.Value` if it represents a default or empty value.
        /// </summary>
        object ToDbValue(object value);

        #endregion ToDbValue - If value is null then return DBNull.Value

        #region SqlNow - Currect Server DateTime

        /// <summary>
        /// Returns current DateTime for SQL parameter.
        /// </summary>
        object SqlNow();

        #endregion SqlNow - Currect Server DateTime
    }
}
