using System;

namespace Contract;

public record TextMessage
{
    public int DocumentId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}