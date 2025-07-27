namespace Helper.Common
{
    public class Enums
    {
        public enum ResponseStatus
        {
            Fail = 0,
            Success = 1,
            DataBlank = 2
        }

        public enum OperationType
        {
            Add = 1,
            Update = 2
        }

        public enum RecordActiveStatus
        {
            Active = 1,
            InActive = 0
        }

        public enum RecordDeleteStatus
        {
            Deleted = 1,
            NotDeleted = 0
        }
    }
}
