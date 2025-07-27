namespace Helper.Common
{
    public class Messages
    {
        public static string NotFound(string placeHolder)
        {
            return $"{placeHolder ?? ""} not found, {placeHolder?.ToLower() ?? ""} may be deleted or inactive.";
        }

        public static string ResponseNotFound(string subject)
        {
            return $"{subject ?? ""} not found. It may be blank or empty.";
        }

        public static string CannotBeEmpty(string obj, string blank)
        {
            return $"{obj ?? ""} cannot be {blank ?? ""}.";
        }

        public static string OperationSuccess(string subject, string operation)
        {
            return $"{subject ?? ""} details {operation ?? ""} successfully.";
        }

        public static string RetrievedSuccess(string subject)
        {
            return $"{subject ?? ""} details retrieved successfully.";
        }

        public static string DeletedSuccess(string subject)
        {
            return $"{subject ?? ""} deleted (deactivated) successfully.";
        }

        public static string SubmitSuccess(string subject)
        {
            return $"{subject ?? ""} submitted successfully.";
        }

        public static string NotSubmitted(string subject)
        {
            return $"{subject ?? ""} not submitted.";
        }

        public static string AlreadyExists(string subject)
        {
            return $"{subject ?? ""} already exists.";
        }

        public static string DataFatchingError(string tableName)
        {
            return $"An error occurred while retrieving data from {tableName ?? ""}";
        }
    }
}
