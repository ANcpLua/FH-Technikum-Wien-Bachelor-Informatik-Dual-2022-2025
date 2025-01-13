using System.Text;
using Contract.Logger;
using ImageMagick;
using Tesseract;

namespace PaperlessServices.TesseractOCR;

public class Ocr : IOcrClient
{
    private readonly string? _language;
    private readonly string? _tessDataPath;
    private readonly IPaperlessLogger _logger;

    public Ocr(string? language, string? tessDataPath, IPaperlessLogger logger)
    {
        _language = language ?? "eng";
        _tessDataPath = tessDataPath ?? "./tessdata";
        _logger = logger;

        MagickNET.Initialize(); // ImageMagick
    }

    public string OcrPdf(Stream inputStream)
    {
        var stringBuilder = new StringBuilder();
        try
        {
            // Image reading settings with desired density
            var settings = new MagickReadSettings
            {
                Density = new Density(300)
            };

            using var images = new MagickImageCollection();
            images.Read(inputStream, settings); // Read PDF as images
            _logger.LogOperation("OCR", "PDF", $"Starting OCR processing for {images.Count} pages");

            // Process each page/image
            foreach (var (image, index) in images.Select((img, index) => (img, index)))
            {
                ProcessImage(image, stringBuilder, index + 1);
            }

            _logger.LogOperation("OCR", "PDF", "OCR processing completed successfully");
            return stringBuilder.ToString().Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError("OCR", "PDF", "Failed to process PDF file", ex);
            throw;
        }
    }

    private void ProcessImage(IMagickImage image, StringBuilder stringBuilder, int pageNumber)
    {
        try
        {
            _logger.LogOperation("OCR", "Image", $"Starting preprocessing for page {pageNumber}");

            // Preprocessing steps to enhance image quality for OCR
            image.Density = new Density(300, 300, DensityUnit.PixelsPerInch);
            image.Format = MagickFormat.Png;
            image.ColorSpace = ColorSpace.Gray;
            image.Alpha(AlphaOption.Remove);
            image.Enhance();
            image.Deskew(new Percentage(40));
            image.Contrast();

            _logger.LogOperation("OCR", "Process", $"Starting TesseractOCR processing for page {pageNumber}");

            // Convert processed image to byte array for TesseractOCR
            var imageData = image.ToByteArray();

            // Initialize TesseractOCR engine and perform OCR
            using var tesseract = new TesseractEngine(_tessDataPath, _language, EngineMode.Default);
            using var pix = Pix.LoadFromMemory(imageData);
            using var page = tesseract.Process(pix);

            var text = page.GetText();
            stringBuilder.AppendLine(text); // Append extracted text

            _logger.LogOperation("OCR", "Image", $"Successfully processed page {pageNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError("OCR", "Image", $"Failed to process page {pageNumber}", ex);
            throw;
        }
    }
}
