namespace DTO.Common.Response
{
    public class GlobalResponseModel<T>
    {
        public static readonly object[] blankArray = Array.Empty<object>();
        public static readonly object returnId = new { Id = 0 };

        public GlobalResponseModel()
        {
            statusCode = 200;
        }

        public bool status { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public Exception? exception { get; set; }
        public T data { get; set; }
    }
}