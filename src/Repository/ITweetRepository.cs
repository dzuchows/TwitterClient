using System.Linq.Expressions;

namespace TwitterClient;

public interface ITweetRepository
{
    Tweet GetById(string id);
    IEnumerable<Tweet> ListTweets();
    IEnumerable<Tweet> ListTweets(Func<Tweet, bool> predicate);
    void Add(Tweet tweet);
    
    Dictionary<string, int> ListHashTags();
    Dictionary<string, int> ListHashTags(Func<KeyValuePair<string, int>, bool> predicate);

}