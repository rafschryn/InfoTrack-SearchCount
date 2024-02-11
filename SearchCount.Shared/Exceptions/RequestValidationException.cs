namespace SearchCount.Shared
{
    public class RequestValidationException : Exception
    {
        public RequestValidationException()
        {
        }

        public RequestValidationException(string message)
            : base(message)
        {
        }
    }
}
