using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TwitterClient.TweetReporter;

public class TweetReporterBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TweetCollectorBackgroundService> _logger;

    public TweetReporterBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<TweetCollectorBackgroundService> logger) =>
        (_serviceProvider, _logger) = (serviceProvider, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(TweetReporterBackgroundService)} is running.");

        await ReportAsync(stoppingToken);
    }

    private async Task ReportAsync(CancellationToken token)
    {
        _logger.LogInformation($"{nameof(TweetReporterBackgroundService)} is reporting.");

        using IServiceScope scope = _serviceProvider.CreateScope();
        ITweetReporterService scopedReportingService =
            scope.ServiceProvider.GetRequiredService<ITweetReporterService>();

        await scopedReportingService.Report(token);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(TweetCollectorBackgroundService)} is stopping.");
        await base.StopAsync(stoppingToken);
    }
}
