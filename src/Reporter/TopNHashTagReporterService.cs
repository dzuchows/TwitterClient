using System.Globalization;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TwitterClient.Reporter;

public class TopNHashTagReporterService : IReporterService
{
    private readonly ILogger<TweetCollectorService> logger;
    private readonly IConfiguration configuration;
    private readonly ITweetRepository repository;
    private readonly IReportWriter writer;

    public TopNHashTagReporterService(ITweetRepository repository, IReportWriter writer, IConfiguration configuration,
        ILogger<TweetCollectorService> logger)
    {
        this.logger = logger;
        this.repository = repository;
        this.configuration = configuration;
        this.writer = writer;
    }

    public void Report(CancellationToken token)
    {
        var configN = configuration["TopNHashTagReporter:Top"];
        if (!int.TryParse(configN, out var topN ))
        {
            logger.LogCritical("Missing or invalid configuration parameter: TopNHashTagReporter:Top");
            throw new ArgumentNullException("TopNHashTagReporter:Top");
        }

        try
        {
            var top10 = repository
                .ListHashTags()
                .OrderByDescending(t => t.Value.Frequency)
                .Take(topN);

            var output = new StringBuilder();
            output.AppendLine(
                $"Top {topN} hash tags as of {DateTime.Now.ToString(CultureInfo.InvariantCulture)}. Tweets processed: {repository.TweetCount()} Hashtags found: {repository.HashTagCount()}");

            foreach (var t in top10)
            {
                output.AppendLine($"{t.Key}:{t.Value.Frequency}");
            }

            writer.Write(output.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{ServiceName} Error while generating report",
                nameof(TopNHashTagReporterService));
            throw;
        }
    }
}