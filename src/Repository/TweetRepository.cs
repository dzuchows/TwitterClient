using System.Collections.Concurrent;

namespace TwitterClient
{
    public class TweetRepository : ITweetRepository
    {
        private readonly List<Tweet> tweets = new();
        private readonly ConcurrentDictionary<string, HashTag> hashTags = new();

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

        public int TweetCount()
        {
            return tweets.Count();
        }

        public void Add(Tweet tweet)
        {
            tweets.Add(tweet);

            var tagsInTweet = tweet.Text.Split(' ').Where(t => t.StartsWith('#')).ToList();
            foreach (var tag in tagsInTweet)
            {
                var hashTag = new HashTag(tag);
                hashTags.AddOrUpdate(tag, hashTag, (key, oldValue) =>
                {
                    oldValue.Frequency++;
                    return oldValue;
                });
            }
        }

        public Dictionary<string, HashTag> ListHashTags()
        {
            return hashTags.ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, HashTag> ListHashTags(Func<KeyValuePair<string, HashTag>, bool> predicate)
        {
            return hashTags.Where(predicate).ToDictionary(x => x.Key, x => x.Value);
        }

        public int HashTagCount()
        {
            return hashTags.Count();
        }
    }
}