using System;
using System.IO;

namespace AutoService_Master.Services;

public static class LoggerService
{
    private static readonly string LogFilePath = "log.txt";

    public static void LogAction(string action)
    {
        try
        {
            string username = AuthService.Instance.CurrentUser?.Username ?? "System";

            string logEntry = $"{DateTime.Now:HH:mm:ss} - [{username}] {action}\n";

            File.AppendAllText(LogFilePath, logEntry);
        }
        catch
        {
        }
    }
}