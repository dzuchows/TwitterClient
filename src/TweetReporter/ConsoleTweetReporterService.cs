using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TwitterClient.TweetReporter;

public class ConsoleTweetReporterService :ITweetReporterService
{
    private readonly ILogger<TweetCollectorService> logger;
    private readonly IConfiguration configuration;
    private readonly ITweetRepository repository;

    public ConsoleTweetReporterService(ITweetRepository repository, IConfiguration configuration,
        ILogger<TweetCollectorService> logger)
    {
        this.logger = logger;
        this.repository = repository;
        this.configuration = configuration;
    }
    public async Task Report(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var top10 = repository.ListHashTags().OrderByDescending(t => t.Value).Take(10);
    
                Console.WriteLine($"Top 10 hash tags as of {DateTime.Now.ToString(CultureInfo.InvariantCulture)}");
                foreach (var t in top10)
                {
                    Console.WriteLine($"{t.Key}:{t.Value}");
                }
        
                await Task.Delay(10_000, token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{ServiceName} Error while processing response", nameof(ConsoleTweetReporterService));
                throw;                
            }
        }
    }
}