namespace SearchCount.Shared
{
    public class CaseNotHandledException : Exception
    {
        public CaseNotHandledException()
        {
        }

        public CaseNotHandledException(string message)
            : base(message)
        {
        }
    }
}
