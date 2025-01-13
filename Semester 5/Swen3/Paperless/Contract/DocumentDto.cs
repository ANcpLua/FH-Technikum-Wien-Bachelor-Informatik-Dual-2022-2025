using System;
using Microsoft.AspNetCore.Http;

namespace Contract;

public record DocumentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime DateUploaded { get; set; }
    public string? OcrText { get; set; }
    public IFormFile? File { get; init; }
}