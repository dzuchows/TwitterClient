namespace TwitterClient.Reporter;

public interface IReportWriter
{
    void Write(string message);
}