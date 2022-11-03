namespace TwitterClient.Reporter;

public class ConsoleReportWriter : IReportWriter
{
    public void Write(string message)
    {
        Console.WriteLine(message);
    }
}