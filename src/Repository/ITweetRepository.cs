namespace TwitterClient;

public interface ITweetRepository
{
    Tweet GetById(string id);
    IEnumerable<Tweet> ListTweets();
    IEnumerable<Tweet> ListTweets(Func<Tweet, bool> predicate);
    int TweetCount();
    void Add(Tweet tweet);
    Dictionary<string, HashTag> ListHashTags();
    Dictionary<string, HashTag> ListHashTags(Func<KeyValuePair<string, HashTag>, bool> predicate);
    int HashTagCount();
}