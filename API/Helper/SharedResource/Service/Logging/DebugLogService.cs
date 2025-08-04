using DTO.Common.Request;
using Helper.SharedResource.Interface.Logging;
using Microsoft.Extensions.Options;
using static System.IO.File;

namespace Helper.SharedResource.Service.Logging
{
    public class DebugLogService : IDebugLogService
    {
        private readonly string _basePath;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        // Corrected constructor parameter type from IOptions<DebugLogService> to IOptions<DebugLogSettings>
        public DebugLogService(IOptions<DebugLogSettings> _iOptions)
        {
            _basePath = _iOptions.Value.basePath; // Correctly access the BasePath property
            if (string.IsNullOrEmpty(_basePath))
            {
                // Use DebugLogSettings instead of ManualLogSettings for clarity
                throw new InvalidOperationException("DebugLogSettings:BasePath is not configured in appsettings.json.");
            }
        }

        // subModuleName is now passed as moduleName to LogContext for consistency with Serilog
        public async Task WriteLogAsync(string subModuleName, string methodName, string message)
        {
            try
            {
                var now = DateTime.Now;

                // Assuming Helper.Common.Enums.ModulesEnum.Master.ToString() provides the main module (e.g., "Master")
                string mainModule = Common.Enums.ProjectName.MedGuardian.ToString();

                // 1. Construct the full directory path according to your structure:
                // BasePath / MainModule / SubModule / Date / log.txt
                string logDirectory = Path.Combine(
                    _basePath,
                    mainModule,                 // Main Module (e.g., "Master")
                    subModuleName,              // Sub Module (e.g., "UserManagement")
                    now.ToString("dd-MM-yyyy")  // Date folder (e.g., "28-07-2025")
                );

                // 2. Ensure the directory exists. This creates all nested folders if they don't exist.
                Directory.CreateDirectory(logDirectory);

                // 3. Define the log file path. We'll use a simple "log.txt" for each day.
                string logFilePath = Path.Combine(logDirectory, $"SeriLog-{now:dd-MM-yyyy}.txt");

                // 4. Format the log entry with a timestamp, method name, and message.
                // Matching the format: [Timestamp] - [MethodName] - Message
                string logEntry = $"[{now:yyyy-MM-dd HH:mm:ss.fff}] - [{methodName}] - {message}{Environment.NewLine}";

                // 5. Use SemaphoreSlim to ensure thread-safe file access.
                await _semaphoreSlim.WaitAsync();
                try
                {
                    // Append the text to the file asynchronously.
                    // This will create the file if it doesn't exist.
                    await AppendAllTextAsync(logFilePath, logEntry);
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            }
            catch (Exception ex)
            {
                // If logging fails, write to the console as a fallback.
                // Changed "moduleName" to "subModuleName" for consistency with the parameter.
                Console.WriteLine($"--- CRITICAL MANUAL LOGGING FAILURE in module '{subModuleName}' ---");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Original Log Message: {message}");
            }
        }
    }
}
