using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UI.Model;

public class Tour
{
    public Tour()
    {
        TourLogs = new List<TourLog>();
        Name = string.Empty;
        Description = string.Empty;
        From = string.Empty;
        To = string.Empty;
        TransportType = string.Empty;
    }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [Required(ErrorMessage = "From city is required")]
    [JsonPropertyName("from")]
    public string From { get; set; }

    [Required(ErrorMessage = "To city is required")]
    [JsonPropertyName("to")]
    public string To { get; set; }

    [JsonPropertyName("imagePath")]
    public string? ImagePath { get; set; }

    [JsonPropertyName("routeInformation")]
    public string? RouteInformation { get; set; }

    [JsonPropertyName("distance")]
    public double? Distance { get; set; }

    [JsonPropertyName("estimatedTime")]
    public double? EstimatedTime { get; set; }

    [Required(ErrorMessage = "Transport type is required")]
    [JsonPropertyName("transportType")]
    public string TransportType { get; set; }

    public List<TourLog> TourLogs { get; set; }

    [JsonIgnore]
    public string Popularity
    {
        get => TourLogs.Count switch
        {
            0 => "Not popular",
            < 2 => "Less popular",
            < 3 => "Moderately popular",
            < 4 => "Popular",
            _ => "Very popular"
        };
    }

    [JsonIgnore]
    public double AverageRating
    {
        get => TourLogs.Count > 0 && TourLogs.Any(x => x.Rating.HasValue)
            ? TourLogs
                .Where(x => x.Rating.HasValue)
                .Average(x => {
                    if (x.Rating != null)
                    {
                        return x.Rating.Value;
                    }
                    return 0;
                })
            : 0;
    }

    [JsonIgnore]
    public bool IsChildFriendly
    {
        get => TourLogs.Count != 0 && TourLogs.All(x => x.Difficulty <= 2) && TourLogs.All(x => x.Rating >= 3);
    }
}
