using System.ComponentModel.DataAnnotations;

namespace PostgreSQL.Entities;

public class Document
{
    public int Id { get; set; }
    
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Content { get; set; } =string.Empty;

    [MaxLength(255)]
    public string FilePath { get; set; } = string.Empty;
    public DateTime DateUploaded { get; set; } = DateTime.UtcNow;
    
    [MaxLength(555)]
    public string? OcrText { get; set; } = string.Empty;
}