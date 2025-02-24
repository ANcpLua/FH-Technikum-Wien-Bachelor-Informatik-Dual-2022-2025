using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.PersistenceModel;

public class TourLogPersistence
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime DateTime { get; set; } = DateTime.Now;

    [Required]
    public string Comment { get; set; } = null!;

    public double? Difficulty { get; set; }
    public double? TotalDistance { get; set; }
    public double? TotalTime { get; set; }
    public double? Rating { get; set; }
    public Guid TourPersistenceId { get; set; }

    [ForeignKey("TourPersistenceId")]
    public TourPersistence TourPersistence { get; set; } = null!;
}