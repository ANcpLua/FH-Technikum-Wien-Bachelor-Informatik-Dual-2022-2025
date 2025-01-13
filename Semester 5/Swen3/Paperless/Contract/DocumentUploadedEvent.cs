using System;

namespace Contract;

public record DocumentUploadedEvent
{
    public int DocumentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
