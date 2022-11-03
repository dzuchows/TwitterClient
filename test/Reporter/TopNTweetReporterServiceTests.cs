using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TwitterClient;
using TwitterClient.Reporter;
using Xunit;

namespace TweetCollector.Test.Reporter
{
    public class TopNTweetReporterServiceTests
    {

        private readonly IConfiguration testConfiguration;

        public TopNTweetReporterServiceTests()
        {
            Dictionary<string, string> settings = new Dictionary<string, string> {
                {"TopNHashTagReporter:Top", "2"},
            };

            testConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }
        [Fact]
        public void should_report_topn_hashtags()
        {
            var mockRepository = new Mock<ITweetRepository>();
            var mockLogger = new Mock<ILogger<TweetCollectorService>>();
            var mockReportWriter = new Mock<IReportWriter>();

            var data = new Dictionary<string, HashTag>();
            data.Add("#HashTag1", new HashTag("#HashTag1", 10));
            data.Add("#HashTag2", new HashTag("#HashTag2", 5));
            data.Add("#HashTag3", new HashTag("#HashTag3", 1));
 
            mockRepository.Setup(x => x.ListHashTags()).Returns(data);
            
            var objectUnderTest = new TopNHashTagReporterService(mockRepository.Object, mockReportWriter.Object,
                testConfiguration, mockLogger.Object);

            objectUnderTest.Report(CancellationToken.None);
            
            mockReportWriter.Verify(x =>
                x.Write(It.Is<string>(s => s.Contains("#HashTag1") && s.Contains("#HashTag2") && !s.Contains("#HashTag3"))));
        }
    
    }
}