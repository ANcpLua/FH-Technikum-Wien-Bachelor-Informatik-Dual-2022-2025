using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.PersistenceModel;

public class TourPersistence
{
    public List<TourLogPersistence> TourLogPersistence { get; set; } = new();

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string From { get; set; } = string.Empty;

    [Required]
    public string To { get; set; } = string.Empty;

    public string? ImagePath { get; set; }
    public string? RouteInformation { get; set; }
    public double? Distance { get; set; }
    public double? EstimatedTime { get; set; }

    [Required]
    public string TransportType { get; set; } = string.Empty;
}
