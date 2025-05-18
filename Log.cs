internal class Log
{
    public void WriteInProductionLog(string text)
    {
        string logFilePath = @"log/prod.log";
        int lines = File.ReadAllLines(logFilePath).Length;
        CreateNewLogFileIfTooBig(logFilePath, lines);
        string logEntry = $"{DateTime.Now}: {text}";
        File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
    }

    private static void CreateNewLogFileIfTooBig(string logFilePath, int lines)
    {
        int maxLines = 500;
        if (lines > maxLines)
        {
            File.Move(logFilePath, @"log/prod_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log");
        }
    }

    public string ReadFromProductionLogAsString(int lines)
    {
        string[] logs = ReadFromProductionLog(lines);
        return string.Join(Environment.NewLine, logs);
    }
    private string[] ReadFromProductionLog(int lines)
    {
        string logFilePath = @"log/prod.log";
        string[] logs = File.ReadAllLines(logFilePath);
        string[] result = logs.Skip(Math.Max(0, logs.Length - lines)).ToArray();
        return result;
    }
}
