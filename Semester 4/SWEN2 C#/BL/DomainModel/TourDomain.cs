namespace BL.DomainModel;

public class TourDomain
{
    public TourDomain()
    {
        Logs = [];
        Name = string.Empty;
        Description = string.Empty;
        From = string.Empty;
        To = string.Empty;
        TransportType = string.Empty;
    }

    public List<TourLogDomain> Logs { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string? ImagePath { get; set; }
    public string? RouteInformation { get; set; }
    public double? Distance { get; set; }
    public double? EstimatedTime { get; set; }
    public string TransportType { get; set; }
}