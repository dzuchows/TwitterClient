using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwitterClient.TweetCollector;

namespace TwitterClient;

public class TweetCollectorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TweetCollectorBackgroundService> _logger;

    public TweetCollectorBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<TweetCollectorBackgroundService> logger) =>
        (_serviceProvider, _logger) = (serviceProvider, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(TweetCollectorBackgroundService)} is running.");

        await DoWorkAsync(stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken token)
    {
        _logger.LogInformation($"{nameof(TweetCollectorBackgroundService)} is working.");

        using IServiceScope scope = _serviceProvider.CreateScope();
        ITweetCollectorService scopedProcessingService =
            scope.ServiceProvider.GetRequiredService<ITweetCollectorService>();

        await scopedProcessingService.Collect(token);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(TweetCollectorBackgroundService)} is stopping.");
        await base.StopAsync(stoppingToken);
    }
}