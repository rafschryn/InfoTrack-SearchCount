namespace SearchCount.Shared.Models
{
    public class SearchCountResponse
    {
        public required IEnumerable<int> Indices { get; set; }
    }
}
