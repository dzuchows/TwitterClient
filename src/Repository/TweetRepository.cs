using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace TwitterClient
{

    public class TweetRepository : ITweetRepository
    {
        private readonly List<Tweet> tweets = new();
        private readonly ConcurrentDictionary<string, int> hashTags = new();

        public Tweet GetById(string id)
        {
            return tweets.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Tweet> ListTweets()
        {
            return tweets.ToList();
        }

        public IEnumerable<Tweet> ListTweets(Func<Tweet, bool> predicate)
        {
            return tweets.Where(predicate).ToList();
        }

        public void Add(Tweet tweet)
        {
            tweets.Add(tweet);

            var tagsInTweet = tweet.Text.Split(' ').Where(t => t.StartsWith('#')).ToList();
            foreach (var tag in tagsInTweet)
            {
                hashTags.AddOrUpdate(tag, 1, (key, oldValue) => oldValue + 1);
            }
        }

        public Dictionary<string, int> ListHashTags()
        {
            return hashTags.ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, int> ListHashTags(Func<KeyValuePair<string, int>, bool> predicate)
        {
            return hashTags.Where(predicate).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}