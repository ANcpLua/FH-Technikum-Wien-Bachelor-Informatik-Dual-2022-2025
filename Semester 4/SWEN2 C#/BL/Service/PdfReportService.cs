using BL.DomainModel;
using BL.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace BL.Service;

public class PdfReportService : IPdfReportService
{
    public PdfReportService() => QuestPDF.Settings.License = LicenseType.Community;

    public byte[] GenerateTourReport(TourDomain tour) => GenerateReport("Tour Report", [tour]);

    public byte[] GenerateSummaryReport(IEnumerable<TourDomain> tours) => GenerateReport("Summary Report", tours);

    private static byte[] GenerateReport(string title, IEnumerable<TourDomain> tours)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Text(title).SemiBold().FontSize(20).AlignCenter();

                page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                {
                    foreach (var tour in tours)
                    {
                        column.Item().Column(tourColumn =>
                        {
                            AddTourDetails(tourColumn, tour);
                            AddTourImage(tourColumn, tour.ImagePath);
                            if (tour.Logs.Count > 0) AddTourLogs(tourColumn, tour.Logs);
                            tourColumn.Item().PaddingVertical(10).LineHorizontal(1);
                        });
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    private static void AddTourDetails(ColumnDescriptor column, TourDomain tour)
    {
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(100);
                columns.RelativeColumn();
            });

            AddTableRow(table, "Tour Name:", tour.Name);
            AddTableRow(table, "Description:", tour.Description);
            AddTableRow(table, "From:", tour.From);
            AddTableRow(table, "To:", tour.To);
            AddTableRow(table, "Distance:", FormatDistance(tour.Distance));
            AddTableRow(table, "Est. Time:", FormatTime(tour.EstimatedTime));
            AddTableRow(table, "Transport:", tour.TransportType);
        });
    }

    private static void AddTableRow(TableDescriptor table, string label, string value)
    {
        table.Cell().Text(label);
        table.Cell().Text(value);
    }

    private static string FormatDistance(double? distance) => 
        distance?.ToString("N2", CultureInfo.CurrentCulture) is { } value 
            ? $"{value} meters" 
            : "N/A";

    private static string FormatTime(double? time) => 
        time?.ToString("N2", CultureInfo.CurrentCulture) is { } value 
            ? $"{value} minutes" 
            : "N/A";

    private static void AddTourImage(ColumnDescriptor column, string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) return;

        try
        {
            column.Item().Image(imagePath).FitWidth();
        }
        catch (Exception ex)
        {
            column.Item().Background(Colors.Grey.Lighten3)
                .Padding(10)
                .Text($"Error loading image: {ex.Message}")
                .FontSize(10);
        }
    }

    private static void AddTourLogs(ColumnDescriptor column, IEnumerable<TourLogDomain> tourLogs)
    {
        column.Item().PaddingTop(10).Text("Tour Logs:").SemiBold();

        foreach (var log in tourLogs)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(100);
                    columns.RelativeColumn();
                });

                AddTableRow(table, "Date:", log.DateTime.ToString("dd.MM.yyyy HH:mm", CultureInfo.CurrentCulture));
                AddTableRow(table, "Comment:", log.Comment ?? "N/A");
                AddTableRow(table, "Difficulty:", log.Difficulty.ToString(CultureInfo.InvariantCulture));
                AddTableRow(table, "Rating:", log.Rating.ToString(CultureInfo.CurrentCulture));
                AddTableRow(table, "Distance:", $"{log.TotalDistance.ToString("N2", CultureInfo.CurrentCulture)} meters");
                AddTableRow(table, "Time:", $"{log.TotalTime.ToString("N2", CultureInfo.CurrentCulture)} minutes");
            });
        }
    }
}