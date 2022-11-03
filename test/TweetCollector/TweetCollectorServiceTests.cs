using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using TwitterClient;
using Xunit;

namespace TweetCollector.Test.TweetCollector
{
    public class TweetCollectorServiceTests
    {
        
        private readonly IConfiguration TestConfiguration;

        public TweetCollectorServiceTests()
        {
            Dictionary<string, string> settings = new Dictionary<string, string> {
                {"TweetCollector:AccessToken", "FakeToken"},
            };

            TestConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        [Fact]
        public async Task adds_tweet_to_repository()
        {
            var httpClient = CreateMockedHttpClient();
            var mockRepository = new Mock<ITweetRepository>();
            var mockLogger = new Mock<ILogger<TweetCollectorService>>();
            
            
            var objectUnderTest = new TweetCollectorService(mockRepository.Object, httpClient, 
                TestConfiguration, mockLogger.Object);
            await objectUnderTest.Collect(CancellationToken.None);

            mockRepository.Verify(r => r.Add(It.IsAny<Tweet>()));

        }
        
        [Fact]
        public async Task should_throw_when_no_access_token()
        {
            var httpClient = CreateMockedHttpClient();
            var mockRepository = new Mock<ITweetRepository>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<TweetCollectorService>>();
            
            
            var objectUnderTest = new TweetCollectorService(mockRepository.Object, httpClient, 
                mockConfiguration.Object, mockLogger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => objectUnderTest.Collect(CancellationToken.None));
        }

        private static HttpClient CreateMockedHttpClient()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(TweetCollectorService.StreamUrl)
                .Respond("application/json", @"{
            'data': { 
                'edit_history_tweet_ids': [ 
                '1588123771377852417'
                    ],
                'id': '1588123771377852417',
                'text': '@hyunlvrz cno ka ba bestie https://t.co/ljyqWfQGAO'}");
     
            return new HttpClient(mockHttp);
        }
    }
}