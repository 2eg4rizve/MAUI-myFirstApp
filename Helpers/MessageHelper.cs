public class MessageHelper
{
    private static MessageHelper instance;
    public static MessageHelper Instance
    {
        get
        {
            if (instance == null)
                instance = new MessageHelper();
            return instance;
        }
    }

    private string text;
    public string Text
    {
        get => text;
        set
        {
            text = value;
            TextUpdated?.Invoke(this, text);
        }
    }

    public event EventHandler<string> TextUpdated;
}
