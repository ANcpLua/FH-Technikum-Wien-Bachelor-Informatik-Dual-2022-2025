using System.Net.Http.Headers;
using Contract;
using EasyNetQ;
using Elastic.Clients.Elasticsearch;
using FluentAssertions;
using Newtonsoft.Json;

namespace Tests.EndToEndTests;

[TestFixture]
public class PaperlessEndToEndTests
{
    private readonly HttpClient _client;
    private readonly IBus _messageBus;
    private readonly ElasticsearchClient _elasticsearch;
    private const string TestPdfName = "HelloWorld";
    private DocumentDto? _uploadedDocument;

    // HttpClient, Message Bus, and Elasticsearch Client
    public PaperlessEndToEndTests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:80") };
        _messageBus = RabbitHutch.CreateBus("host=localhost;port=5672;username=guest;password=guest");
        _elasticsearch = new ElasticsearchClient(new ElasticsearchClientSettings(new Uri("http://localhost:9200")));
    }

    [Test, Order(1)]
    public async Task UploadDocument_ToPaperlessSystem_StoresInMinio()
    {
        // Arrange
        var testPdfPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "EndToEndTests", "HelloWorld.pdf");
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(TestPdfName), "name");
        form.Add(new StreamContent(File.OpenRead(testPdfPath))
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/pdf") }
        }, "file", "HelloWorld.pdf");

        // Act
        var response = await _client.PostAsync("/documents/upload", form);
        var document = JsonConvert.DeserializeObject<DocumentDto>(
            await response.Content.ReadAsStringAsync());

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        document.Should().NotBeNull();
        
        // Store the uploaded document for subsequent tests
        _uploadedDocument = document;
    }

    [Test, Order(2)]
    public async Task ProcessDocument_WithTesseract_ExtractsText()
    { 
        // Arrange
        
        // 30-second timeout to prevent indefinite waiting
        using var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        // Store the OCR result received from the message bus
        TextMessage? ocrResult = null;
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        // Subscribe to the 'document.processed' topic to listen for OCR completion messages
        await _messageBus.PubSub.SubscribeAsync<TextMessage>(
            subscriptionId: $"ocr_worker_{environment}", // Unique subscription ID based on the environment
            onMessage: (message, _) =>
            {
                // Check if the uploaded document exists and if the message corresponds to it
                if (_uploadedDocument == null || message.DocumentId != _uploadedDocument.Id) return Task.CompletedTask;

                // Assign the received message to ocrResult
                ocrResult = message;

                // Cancel the token to signal that the OCR result has been received
                tokenSource.Cancel();

                return Task.CompletedTask;
            },
            configure: x => x.WithTopic("document.processed"), // Specify the topic to subscribe to
            cancellationToken: tokenSource.Token); // Pass the cancellation token

        // Act
        if (_uploadedDocument != null)
        {
            // Publish a DocumentUploadedEvent to trigger the OCR processing workflow
            await _messageBus.PubSub.PublishAsync(new DocumentUploadedEvent
            {
                DocumentId = _uploadedDocument.Id,
                FileName = _uploadedDocument.FilePath,
                UploadedAt = DateTime.UtcNow
            }, cancellationToken: tokenSource.Token);

            try
            {
                // Wait for the OCR completion message or timeout
                while (ocrResult == null && !tokenSource.IsCancellationRequested)
                {
                    await Task.Delay(500, tokenSource.Token);
                }
            }
            catch (OperationCanceledException) when (ocrResult != null)
            {
                // Expected when we cancel after receiving the message
            }
  
            // Assert: Check if the OCR result is not null and corresponds to the uploaded document
            ocrResult.Should().NotBeNull();
            ocrResult!.DocumentId.Should().Be(_uploadedDocument.Id);
        }

        ocrResult?.Text.Should().Contain("Hello");
    }

    [Test, Order(3)]
    public async Task SearchDocument_InElasticsearch_FindsProcessedDocument()
    {
        // Allow indexing to complete
        await Task.Delay(2000);

   
        // Act: Perform a search query in the 'paperless-documents' index for the term "Hello"
        var searchResult = await _elasticsearch.SearchAsync<DocumentDto>(s => s
            .Index("paperless-documents") // Specify the Elasticsearch index
            .Query(q => q.MultiMatch(mm => mm
                .Query("Hello") // Search query term
                .Fields(new[] { "name", "ocrText" })))); // Fields to search

  
        // Assert: Ensure that at least one document is found in the search results
        searchResult.Documents.Should().NotBeEmpty();
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        // Dispose the message bus connection to RabbitMQ
        _messageBus.Dispose();
        // Dispose the HttpClient instance
        _client.Dispose();
    }
}