using System.Text.Json.Serialization;

namespace API.Domain
{
    public class TodoApp
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonPropertyName("Descricao")]
        public string Descricao { get; set; }
        [JsonPropertyName("Data")]
        public DateTime Data { get; set; }
        [JsonPropertyName("Status")]
        public string Status { get; set; }
    }
}
