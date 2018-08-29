namespace Demian
{
    public sealed class ConstText : IText
    {
        public ConstText(string content)
        {
            Content = content;
        }

        public string Content { get; }
    }
}