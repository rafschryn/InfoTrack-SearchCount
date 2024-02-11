
using System.Text.Json.Serialization;

namespace SearchCount.Shared.Models
{
    public class SearchCountRequest
    {
        public required string SearchTerm { get; set; }
        public required string Url { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required SearchEngine SearchEngine { get; set; }
    }
}
