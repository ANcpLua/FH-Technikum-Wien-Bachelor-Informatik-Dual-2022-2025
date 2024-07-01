using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UI.Model;

public class TourLog
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("dateTime")]
    public DateTime DateTime { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Comment is required")]
    [JsonPropertyName("comment")]
    public string? Comment { get; set; } = string.Empty;

    [Required(ErrorMessage = "Difficulty is required")]
    [JsonPropertyName("difficulty")]
    public double? Difficulty { get; set; }

    [Required(ErrorMessage = "Total Distance is required")]
    [JsonPropertyName("totalDistance")]
    public double? TotalDistance { get; set; }

    [Required(ErrorMessage = "Total Time is required")]
    [JsonPropertyName("totalTime")]
    public double? TotalTime { get; set; }

    [Required(ErrorMessage = "Rating is required")]
    [JsonPropertyName("rating")]
    public double? Rating { get; set; }

    public Guid TourId { get; set; }
}
