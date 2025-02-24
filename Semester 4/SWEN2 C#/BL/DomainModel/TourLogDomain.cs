namespace BL.DomainModel;

public class TourLogDomain
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public string? Comment { get; set; }
    public double Difficulty { get; set; }
    public double TotalDistance { get; set; }
    public double TotalTime { get; set; }
    public double Rating { get; set; }
    public Guid TourDomainId { get; set; }
}