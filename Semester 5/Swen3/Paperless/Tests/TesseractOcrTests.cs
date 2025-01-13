using Moq;
using Contract.Logger;
using Microsoft.Extensions.Logging;
using PaperlessServices.TesseractOCR;

namespace Tests;
[TestFixture]
public class OcrTests
{
    private Ocr _ocr;
    private IPaperlessLogger _paperlessLogger;
    private Mock<ILogger> _mockLogger;
    private const string TestPdfName = "HelloWorld.pdf";
    private string _tessDataPath;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Set up tessdata directory in the test directory
        _tessDataPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "tessdata");
        Directory.CreateDirectory(_tessDataPath);

        // Verify tessdata directory exists and contains required language files
        var engDataFile = Path.Combine(_tessDataPath, "eng.traineddata");
        if (!File.Exists(engDataFile))
        {
            Assert.Fail($"Tesseract language file not found at: {engDataFile}. " +
                       "Please ensure you have downloaded the language files from https://github.com/tesseract-ocr/tessdata " +
                       "and placed them in the tessdata directory.");
        }
    }

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger>();
        _paperlessLogger = new PaperlessLogger(_mockLogger.Object, "TestComponent", "TestEnvironment");
        _ocr = new Ocr("eng", _tessDataPath, _paperlessLogger);
    }

    [TearDown]
    public void TearDown()
    {
        _ocr = null!;
    }

    [Test]
    public void OcrPdf_ValidPdfStream_ReturnsText()
    {
        // Arrange
        var pdfPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "EndToEndTests", TestPdfName);
        Assert.That(File.Exists(pdfPath), Is.True, $"Test file not found: {pdfPath}");

        using var pdfStream = new MemoryStream(File.ReadAllBytes(pdfPath));

        // Act & Assert
        Assert.DoesNotThrow(() => 
        {
            var result = _ocr.OcrPdf(pdfStream);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }, "OCR processing should not throw an exception");

        // Verify logging
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Starting OCR processing for")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.AtLeastOnce);

        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("OCR processing completed successfully")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Test]
    public void OcrPdf_InvalidPdfStream_ThrowsException()
    {
        // Arrange
        var pdfStream = new MemoryStream("This is not a PDF file"u8.ToArray());

        // Act & Assert
        Assert.That(() => _ocr.OcrPdf(pdfStream), Throws.Exception);
    }

    [Test]
    public void OcrPdf_NullPdfStream_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _ocr.OcrPdf(null!));
    }
}
