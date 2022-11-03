namespace TwitterClient
{
    public class Tweet
    {
        public Tweet(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public string Id
        {
            get;
        }

        public string Text
        {
            get;
        }
    }
}