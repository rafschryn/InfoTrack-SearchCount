namespace SearchCount.Shared.DbModels
{
    public class DbSearchCountHistory
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string SearchTerm { get; set; } = string.Empty;
        public string SearchEngine { get; set;} = string.Empty;
        public string Indices { get; set; } = string.Empty;
        public DateTimeOffset DateOfExcecution { get; set; }
    }
}
