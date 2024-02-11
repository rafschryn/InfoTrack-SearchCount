using System.Text.Json.Serialization;

namespace SearchCount.Shared.Models
{
    public class SearchCountHistory
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string SearchTerm { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SearchEngine SearchEngine { get; set; }
        public IEnumerable<int> Indices { get; set; } = Enumerable.Empty<int>();
        public DateTimeOffset DateOfExcecution { get; set; }
    }
}
