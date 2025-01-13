namespace PaperlessServices.Entities;

public class BlDocument
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Content { get; set; } = string.Empty; 
    public string FilePath { get; set; } = default!;
    public DateTime DateUploaded { get; set; }
    public string? OcrText { get; set; }
    public IFormFile? File { get; set; }
}