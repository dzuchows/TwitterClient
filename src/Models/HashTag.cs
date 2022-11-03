namespace TwitterClient;

public class HashTag
{
    public HashTag() : this(String.Empty, 0)
    {
    }
    
    public HashTag(string text) : this(text, 0)
    {
    }

    public HashTag(string text, int frequency)
    {
        Text = text;
        Frequency = frequency;
    }

    public int Frequency
    {
        set;
        get;
    }

    public string Text
    {
        get;
    }
}