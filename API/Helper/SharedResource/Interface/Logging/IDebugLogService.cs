namespace Helper.SharedResource.Interface.Logging
{
    public interface IDebugLogService
    {
        Task WriteLogAsync(string subModuleName, string methodName, string message);
    }
}
