namespace TwitterClient;

public interface ITweetCollectorService
{
    Task Collect(CancellationToken token);
}