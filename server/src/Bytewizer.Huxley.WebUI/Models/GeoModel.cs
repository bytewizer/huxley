using System.Text.Json.Serialization;

namespace Bytewizer.Huxley.Api.Models
{
    public partial class GeoModel
    {
        public GeoModel(List<Feature> features)
        {
            Features = features;
        }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "FeatureCollection";

        [JsonPropertyName("features")]
        public List<Feature> Features { get; private set; }
    }

    public partial class Feature
    {
        public Feature(Properties properties, Geometry geometry)
        {
            Properties = properties;
            Geometry = geometry;
        }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "Feature";

        [JsonPropertyName("properties")]
        public Properties Properties { get; private set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; private set; }
    }

    public partial class Geometry
    {
        public Geometry(double latitude, double longitude)
        {
            Coordinates = new double[] { longitude, latitude };
        }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "Point";

        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; private set; }
    }

    public partial class Properties
    {
        public Properties(string id)
        {
            Id = id;
        }

        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("location")]
        public string Location { get; set; } = "Unknown";

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
