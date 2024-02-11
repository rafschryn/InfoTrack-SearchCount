namespace SearchCount.Shared
{
    public static class Constants
    {
        public const string DefaultSearchResultCount = "101";
        public const string GoogleRegex = @"(?<=<div class=""egMi0 kCrYT""><a href=""/url\?q=)[^""]*";
        public const string BingRegex = @"<a class=""tilk"" href=""http(.*?)""";
    }
}
