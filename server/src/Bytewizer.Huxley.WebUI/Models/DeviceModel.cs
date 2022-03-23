using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Bytewizer.Huxley.Api.Models
{
    public class DeviceModel
    {
        [Key]
        [JsonPropertyName("deviceid")]
        public string DeviceID { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
