namespace TwitterClient.TweetReporter;

public interface ITweetReporterService
{
    Task Report(CancellationToken token);
}