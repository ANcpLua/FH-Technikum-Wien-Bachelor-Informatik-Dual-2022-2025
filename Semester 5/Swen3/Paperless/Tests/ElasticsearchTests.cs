using Contract;
using Elastic.Clients.Elasticsearch;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Tests;

[TestFixture]
public class ElasticsearchTests
{
    private ElasticsearchClient _elasticsearchClient;
    private IConfiguration _configuration;
    private ILogger<ElasticsearchTests> _logger;
    private const string TestIndex = "paperless-documents";
    private const int TestDocumentId = 1;
    private DocumentDto _testDocument;

    [SetUp]
    public async Task Setup()
    {
        // Load configuration from JSON file and environment variables
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("service-appsettings.json")
            .AddEnvironmentVariables();

        _configuration = configBuilder.Build();

        // Initialize Elasticsearch client with the specified URI and default index
        var elasticUri = _configuration["Elasticsearch:Uri"] ?? "http://localhost:9200";
        var settings = new ElasticsearchClientSettings(new Uri(elasticUri))
            .DefaultIndex(TestIndex);

        _elasticsearchClient = new ElasticsearchClient(settings);

        // Initialize logger to output to the console
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        _logger = loggerFactory.CreateLogger<ElasticsearchTests>();

        // Check if the index exists; if not, create it with default settings
        var indexExistsResponse = await _elasticsearchClient.Indices.ExistsAsync(TestIndex);
        if (!indexExistsResponse.Exists)
        {
            var createIndexResponse = await _elasticsearchClient.Indices.CreateAsync(TestIndex);
            createIndexResponse.ElasticsearchServerError.Should().BeNull(
                $"Failed to create index '{TestIndex}': {createIndexResponse.DebugInformation}"
            );
            _logger.LogInformation($"Created Elasticsearch index: {TestIndex}");
        }

        // Prepare a test document to be used in the tests
        _testDocument = new DocumentDto
        {
            Id = TestDocumentId,
            Name = "Test Document",
            OcrText = "This is a sample OCR text for testing."
        };
    }

    [Test, Order(1)]
    public async Task IndexDocument_ToElasticsearch_Succeeds()
    {
        // Act: Index the test document into Elasticsearch
        var indexResponse = await _elasticsearchClient.IndexAsync(_testDocument, idx => idx
                .Index(TestIndex)
                .Id(_testDocument.Id.ToString())
                .Refresh(Refresh.True) // Ensure the document is immediately searchable
        );

        // Assert: Verify that there were no server-side errors during indexing
        indexResponse.ElasticsearchServerError.Should().BeNull(
            $"Indexing failed: {indexResponse.DebugInformation}"
        );
        _logger.LogInformation($"Indexed document with ID: {_testDocument.Id}");

        // Verify the document exists in Elasticsearch by retrieving it
        var getResponse = await _elasticsearchClient.GetAsync<DocumentDto>(_testDocument.Id, g => g
            .Index(TestIndex)
        );

        // Assert: Confirm that the document was found
        getResponse.Found.Should().BeTrue("Document was not found after indexing.");
        _logger.LogInformation($"Verified document exists in Elasticsearch with ID: {_testDocument.Id}");
    }

    [Test, Order(2)]
    public async Task SearchDocument_InElasticsearch_Succeeds()
    {
        // Arrange: Define the search query
        var searchQuery = "sample OCR text";

        // Act: Perform a search query on the 'name' and 'ocrText' fields
        var searchResponse = await _elasticsearchClient.SearchAsync<DocumentDto>(s => s
            .Index(TestIndex)
            .From(0)
            .Size(10)
            .Query(q => q
                .MultiMatch(mm => mm
                    .Query(searchQuery)
                    .Fields(new[] { "name", "ocrText" })
                )
            )
        );

        // Assert: Ensure no server-side errors occurred during the search
        searchResponse.ElasticsearchServerError.Should().BeNull(
            $"Search failed: {searchResponse.DebugInformation}"
        );

        // Assert: Verify that the search returned results
        searchResponse.Documents.Should().NotBeEmpty("No documents found in search results.");

        // Assert: Check that the specific test document is present in the search results
        searchResponse.Documents.Should().ContainSingle(
            d => d.Id == _testDocument.Id,
            "Document not found in search results."
        );
        _logger.LogInformation($"Found document in search results with ID: {_testDocument.Id}");
    }

    [Test, Order(3)]
    public async Task DeleteDocument_FromElasticsearch_Succeeds()
    {
        // Act: Delete the test document from Elasticsearch
        var deleteResponse = await _elasticsearchClient.DeleteAsync<DocumentDto>(_testDocument.Id, d => d
                .Index(TestIndex)
                .Refresh(Refresh.True) // Ensure the deletion is immediately reflected
        );

        // Assert: Verify that there were no server-side errors during deletion
        deleteResponse.ElasticsearchServerError.Should().BeNull(
            $"Deletion failed: {deleteResponse.DebugInformation}"
        );
        _logger.LogInformation($"Deleted document with ID: {_testDocument.Id}");

        // Verify that the document no longer exists in Elasticsearch
        var getResponse = await _elasticsearchClient.GetAsync<DocumentDto>(_testDocument.Id, g => g
            .Index(TestIndex)
        );

        // Assert: Confirm that the document was not found after deletion
        getResponse.Found.Should().BeFalse("Document still exists after deletion.");
        _logger.LogInformation($"Verified document no longer exists in Elasticsearch with ID: {_testDocument.Id}");
    }

    /// <summary>
    /// Cleanup method to ensure the test document is deleted after all tests have run.
    /// </summary>
    [OneTimeTearDown]
    public async Task Cleanup()
    {
        var deleteResponse = await _elasticsearchClient.DeleteAsync<DocumentDto>(_testDocument.Id, d => d
            .Index(TestIndex)
            .Refresh(Refresh.True)
        );

        if (deleteResponse.ElasticsearchServerError == null ||
            deleteResponse.ElasticsearchServerError.Status == 404)
        {
            _logger.LogInformation($"Cleanup: Ensured document with ID {_testDocument.Id} is deleted.");
        }
        else
        {
            _logger.LogWarning(
                $"Cleanup: Failed to delete document with ID {_testDocument.Id}: {deleteResponse.DebugInformation}");
        }
    }
}
