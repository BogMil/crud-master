using System.Text.Json.Serialization;

namespace CrudMaster.Utils
{
    public class OrderProperties
    {
        [JsonPropertyName("col")]
        public string Column { get; set; }
        [JsonPropertyName("dir")]
        public string Direction { get; set; } = OrderDirections.Ascending;
    }
}
