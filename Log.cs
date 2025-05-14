internal class Log
{
    public void Write(string text)
    {
        string logFilePath = @"log/prod.log";
        string logEntry = $"{DateTime.Now}: {text}";
        File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
    }

    public string ReadJoined(int lines)
    {
        string[] logs = Read(lines);
        return string.Join(Environment.NewLine, logs);
    }
    private string[] Read(int lines)
    {
        string logFilePath = @"log/prod.log";
        string[] logs = File.ReadAllLines(logFilePath);
        string[] result = logs.Skip(Math.Max(0, logs.Length - lines)).ToArray();
        return result;
    }
}
