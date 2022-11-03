namespace TwitterClient.Reporter;

public interface IReporterService
{
    void Report(CancellationToken token);
}