namespace TwitterClient.TweetCollector;

public interface ITweetCollectorService
{
    Task Collect(CancellationToken token);
}