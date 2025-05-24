internal class Log
{
    private Dictionary<LogType, string> FilePath = new Dictionary<LogType, string>
    {
        { LogType.Prod, @"log/prod.log" },
        { LogType.Debug, @"log/debug.log" },
        { LogType.Error, @"log/error.log" }
    };

    public void WriteToLog(LogType logType, string text)
    {
        string logFilePath = FilePath[logType];
        if (!File.Exists(logFilePath))
        {
            File.WriteAllText(logFilePath, string.Empty);
        }
        int lines = File.ReadAllLines(logFilePath).Length;
        CreateNewLogFileIfTooBig(logFilePath, lines);
        string logEntry = $"{DateTime.Now} {text}";
        File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
    }

    private static void CreateNewLogFileIfTooBig(string logFilePath, int lines)
    {
        int maxLines = 500;
        if (lines > maxLines)
        {
            File.Move(
                logFilePath,
                logFilePath.Insert(logFilePath.Length - 4, "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"))
            );
        }
    }
    public List<string> ReadLog(LogType logType, int lines)
    {
        string logFilePath = FilePath[logType];
        List<string> logs = File.ReadAllLines(logFilePath).ToList();
        List<string> result = logs.Skip(Math.Max(0, logs.Count - lines)).ToList();
        return result;
    }
}
