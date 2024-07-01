using BL.DomainModel;
using BL.Interface;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace BL.Service;

public class PdfReportService : IPdfReportService
{
    private const string DefaultFontName = "Verdana";
    private const double DefaultFontSize = 12;
    private const double ImageWidth = 10;
    private const double ImageHeight = 7.5;
    private const int SeparatorLength = 50;
    private const string NaValue = "N/A";

    public byte[] GenerateTourReport(TourDomain tour) => GenerateReport("Tour Report", new[]
    {
        tour
    });

    public byte[] GenerateSummaryReport(IEnumerable<TourDomain> tours) => GenerateReport("Summary Report", tours);

    private static byte[] GenerateReport(string title, IEnumerable<TourDomain> tours)
    {
        var document = new Document();
        var section = document.AddSection();
        var font = new Font(DefaultFontName, Unit.FromPoint(DefaultFontSize));

        AddHeader(section, title);

        foreach (var tour in tours)
        {
            AddTourDetails(section, tour, font);
            AddTourImage(section, tour.ImagePath);
            AddTourLogs(section, tour.Logs, font);
            AddSeparator(section);
        }

        return RenderDocument(document);
    }

    private static void AddHeader(Section section, string title)
    {
        var paragraph = section.AddParagraph(title);
        paragraph.Format.Alignment = ParagraphAlignment.Center;
        paragraph.Format.SpaceAfter = Unit.FromPoint(20);
    }

    private static void AddTourDetails(Section section, TourDomain tour, Font font)
    {
        string[] details =
        {
            $"Tour Name: {tour.Name}", $"Description: {tour.Description}", $"From: {tour.From}", $"To: {tour.To}", $"Distance: {tour.Distance?.ToString() ?? NaValue} meters", $"Estimated Time: {tour.EstimatedTime?.ToString() ?? NaValue} minutes", $"Transport Type: {tour.TransportType}"
        };

        foreach (var detail in details)
        {
            AddParagraph(section, detail, font);
        }
    }

    private static void AddTourImage(Section section, string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return;
        }

        try
        {
            var image = section.AddImage(imagePath);
            image.Width = Unit.FromCentimeter(ImageWidth);
            image.Height = Unit.FromCentimeter(ImageHeight);
            image.LockAspectRatio = true;
            section.AddParagraph().Format.SpaceAfter = Unit.FromPoint(20);
        }
        catch (Exception ex)
        {
            AddParagraph(
            section,
            $"Error loading image: {ex.Message}",
            new Font(DefaultFontName, Unit.FromPoint(10))
            );
        }
    }

    private static void AddTourLogs(
        Section section,
        IEnumerable<TourLogDomain>? tourLogs,
        Font font
    )
    {
        if (tourLogs == null)
        {
            return;
        }

        var paragraph = section.AddParagraph("Tour Logs:");
        paragraph.Format.SpaceBefore = Unit.FromPoint(20);
        paragraph.Format.SpaceAfter = Unit.FromPoint(10);

        foreach (var log in tourLogs)
        {
            AddParagraph(section, $"Comment: {log.Comment}", font);
            AddParagraph(
            section,
            $"Date: {log.DateTime:dd.MM.yyyy HH:mm}, Difficulty: {log.Difficulty}, Rating: {log.Rating}",
            font
            );
            AddParagraph(
            section,
            $"Total Distance: {log.TotalDistance} meters, Total Time: {log.TotalTime} minutes",
            font
            );
            AddSeparator(section);
        }
    }

    private static void AddSeparator(Section section)
    {
        var paragraph = section.AddParagraph(new string('-', SeparatorLength));
        paragraph.Format.SpaceBefore = Unit.FromPoint(10);
        paragraph.Format.SpaceAfter = Unit.FromPoint(10);
    }

    private static void AddParagraph(Section section, string text, Font font)
    {
        var paragraph = section.AddParagraph(text);
        paragraph.Format.Font = font.Clone();
        paragraph.Format.SpaceAfter = Unit.FromPoint(10);
    }

    private static byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };
        renderer.RenderDocument();
        using var stream = new MemoryStream();
        renderer.PdfDocument.Save(stream, false);
        return stream.ToArray();
    }
}
