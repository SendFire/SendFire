namespace SendFire.Common.ExtensionMethods
{
    public static class CharExtensions
    {
        public static string MakeLine(this char character, int lineLength) => new string(character, lineLength);

        public static string MakeTitleLine(this char character, string title) => new string(character, title.Length);
    }
}
