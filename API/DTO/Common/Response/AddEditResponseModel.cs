namespace DTO.Common.Response
{
    public class AddEditResponseModel<T>
    {
        public T? data { get; set; }
        public bool status { get; set; }
        public string? message { get; set; }
        public Exception? exception { get; set; }
    }
}