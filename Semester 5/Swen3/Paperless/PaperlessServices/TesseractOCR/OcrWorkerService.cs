using Contract;
using Contract.Logger;
using EasyNetQ;
using PaperlessServices.BL;
using PaperlessServices.MinIoStorage;

namespace PaperlessServices.TesseractOCR;

public class OcrWorkerService : BackgroundService
{
    private readonly IBus _messageBus;
    private readonly IServiceProvider _serviceProvider;
    private readonly IPaperlessLogger _logger;
    private readonly string _subscriptionId;

    public OcrWorkerService(
        IBus messageBus,
        IServiceProvider serviceProvider,
        IPaperlessLogger logger)
    {
        _messageBus = messageBus;
        _serviceProvider = serviceProvider;
        _logger = logger; 
        _subscriptionId = $"ocr_worker_{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogOperation("Worker", "Start", "Starting OCR Worker Service");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Subscribe to the 'document.uploaded' topic with a unique subscription ID
                // HandleDocumentAsync is the callback method invoked when a message is received
                await _messageBus.PubSub.SubscribeAsync<DocumentUploadedEvent>(
                    _subscriptionId,
                    HandleDocumentAsync,
                    x => x.WithTopic("document.uploaded"),
                    stoppingToken);

                // Wait indefinitely until the service is stopped
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Gracefully exit the loop if cancellation is requested
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError("Worker", "Error", "Service error, retrying in 5s", ex);
                await Task.Delay(5000, stoppingToken); 
            }
        }
    }

    private async Task HandleDocumentAsync(DocumentUploadedEvent message, CancellationToken token)
    {
        // Create a new scope to ensure scoped services are disposed properly after processing
        using var scope = _serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            _logger.LogOperation("OCR", "Process", $"Starting document {message.DocumentId}");

            // Retrieve the storage service to access the uploaded document file
            var storageService = services.GetRequiredService<IMinioStorageService>();
            var documentStream = await storageService.GetFileAsync(message.FileName, token);

            // Perform OCR on the document stream
            var ocrClient = services.GetRequiredService<IOcrClient>();
            var extractedText = await PerformOcrAsync(documentStream, message.FileName, ocrClient);

            // Validate that OCR produced some text
            if (string.IsNullOrWhiteSpace(extractedText))
                throw new InvalidOperationException("OCR produced no text output");

            // Retrieve and update the document record with the extracted OCR text
            var documentService = services.GetRequiredService<IDocumentService>();
            var document = await documentService.GetDocument(message.DocumentId, token);
            document.OcrText = extractedText;
            await documentService.UpdateDocument(document, token);

            // Publish a success result message
            await PublishResultAsync(message.DocumentId, extractedText, false, null, DateTime.UtcNow, token);
            _logger.LogOperation("OCR", "Complete", $"Processed document {message.DocumentId}");
        }
        catch (Exception ex)
        {
            _logger.LogError("OCR", "Failed", $"Document {message.DocumentId} processing failed", ex);
            await PublishResultAsync(message.DocumentId, ex.Message, true, ex, DateTime.UtcNow, token);
        }
    }

    private static async Task<string> PerformOcrAsync(Stream documentStream, string fileName, IOcrClient ocrClient)
    {
        if (!Path.GetExtension(fileName).Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
            throw new NotSupportedException("Only PDF files are supported");

        // Reset the stream position to the beginning before reading
        documentStream.Position = 0;

        // Perform OCR in a separate task to avoid blocking the main thread
        return await Task.Run(() => ocrClient.OcrPdf(documentStream));
    }

    private async Task PublishResultAsync(int documentId, string content, bool isError, Exception? ex, DateTime processedAt,
        CancellationToken token)
    {
        // Create the message to be published with the extracted text or error details
        var message = new TextMessage
        {
            DocumentId = documentId,
            Text = isError ? $"{content}\n{ex?.StackTrace}" : content,
            ProcessedAt = processedAt
        };

        // Determine the topic based on whether the processing was successful or resulted in an error
        var topic = isError ? "document.processing.failed" : "document.processed";
        await _messageBus.PubSub.PublishAsync(message, topic, token);
    }
}