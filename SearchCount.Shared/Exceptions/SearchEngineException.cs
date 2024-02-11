namespace SearchCount.Shared
{
    public class SearchEngineException : Exception
    {
        public SearchEngineException()
        {
        }

        public SearchEngineException(string message)
            : base(message)
        {
        }

        public SearchEngineException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
