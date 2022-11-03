using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitterClient;

public class TweetCollectorService : ITweetCollectorService
{
    private readonly ILogger<TweetCollectorService> logger;
    private readonly IConfiguration configuration;
    private readonly ITweetRepository repository;
    private readonly HttpClient client;

    public const string StreamUrl = "https://api.twitter.com/2/tweets/sample/stream";

    public TweetCollectorService(ITweetRepository repository, HttpClient client, IConfiguration configuration,
        ILogger<TweetCollectorService> logger)
    {
        this.logger = logger;
        this.repository = repository;
        this.configuration = configuration;
        this.client = client;
    }

    public async Task Collect(CancellationToken token)
    {
        logger.LogInformation("{ServiceName} started", nameof(TweetCollectorService));

        var access_token = configuration["TweetCollector:AccessToken"];
        if (string.IsNullOrEmpty(access_token))
        {
            logger.LogCritical("Missing a required configuration parameter: TweetCollector:AccessToken");
            throw new ArgumentNullException("TweetCollector:AccessToken");
        }

        var serializer = new JsonSerializer();

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, StreamUrl);
            request.Headers.Add("Authorization", $"Bearer {access_token}");
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);

            await using Stream streamToReadFrom = await response.Content.ReadAsStreamAsync(token);
            using var sr = new StreamReader(streamToReadFrom);
            await using var jsonTextReader = new JsonTextReader(sr);
            jsonTextReader.SupportMultipleContent = true;
            while (await jsonTextReader.ReadAsync(token) && !token.IsCancellationRequested)
            {
                try
                {
                    var serialized = serializer.Deserialize<JObject>(jsonTextReader);
                    if (serialized != null)
                    {
                        repository.Add(JsonToTweet(serialized));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{ServiceName} Error while processing response", nameof(TweetCollectorService));
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{ServiceName} Error while processing response", nameof(TweetCollectorService));
            throw;
        }
    }

    private static Tweet JsonToTweet(JObject json)
    {
        var data = json.Value<JObject>("data");
        return new Tweet(data.Value<string>("id"),
            data.Value<string>("text"));
    }
}