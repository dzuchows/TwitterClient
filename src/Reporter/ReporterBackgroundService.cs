using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TwitterClient.Reporter;

public class ReporterBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TweetCollectorBackgroundService> _logger;

    public ReporterBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<TweetCollectorBackgroundService> logger) =>
        (_serviceProvider, _logger) = (serviceProvider, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(ReporterBackgroundService)} is running.");

        await ReportAsync(stoppingToken);
    }

    private async Task ReportAsync(CancellationToken token)
    {
        _logger.LogInformation($"{nameof(ReporterBackgroundService)} is reporting.");

        using IServiceScope scope = _serviceProvider.CreateScope();
        IReporterService scopedReportingService =
            scope.ServiceProvider.GetRequiredService<IReporterService>();

        while (!token.IsCancellationRequested)
        {
            scopedReportingService.Report(token);
            await Task.Delay(10_000, token);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(TweetCollectorBackgroundService)} is stopping.");
        await base.StopAsync(stoppingToken);
    }
}