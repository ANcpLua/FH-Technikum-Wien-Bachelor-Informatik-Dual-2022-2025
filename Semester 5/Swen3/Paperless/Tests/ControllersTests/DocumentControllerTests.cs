using AutoMapper;
using Contract;
using Contract.Logger;
using EasyNetQ;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaperlessREST.Controllers;
using PaperlessServices.BL;
using PaperlessServices.MinIoStorage;

namespace Tests.ControllersTests;

public class DocumentControllerTests
{
    private DocumentController _controller;
    private Mock<IDocumentService> _documentService;
    private Mock<IMinioStorageService> _storageService;
    private Mock<IPaperlessLogger> _logger;
    private Mock<ElasticsearchClient> _elasticClient;
    private Mock<IBus> _bus;
    private Mock<IMapper> _mapper;
    private Mock<IPubSub> _pubSub;
    private CancellationToken _cancellationToken;

    [SetUp]
    public void Setup()
    {
        SetupMocks();
        CreateController();
        _cancellationToken = CancellationToken.None;
    }

    private void SetupMocks()
    {
        _documentService = new Mock<IDocumentService>();
        _storageService = new Mock<IMinioStorageService>();
        _logger = new Mock<IPaperlessLogger>();
        _elasticClient = new Mock<ElasticsearchClient>();
        _bus = new Mock<IBus>();
        _mapper = new Mock<IMapper>();
        _pubSub = new Mock<IPubSub>();
        
        _bus.Setup(x => x.PubSub).Returns(_pubSub.Object);
    }

    private void CreateController()
    {
        _controller = new DocumentController(
            _documentService.Object,
            _storageService.Object,
            _logger.Object,
            _elasticClient.Object,
            _bus.Object,
            _mapper.Object
        );
    }

    [Test]
    public async Task Upload_ValidInput_ReturnsOkWithDocumentDto()
    {
        // Arrange
        var document = CreateTestDocument();
        var file = new Mock<IFormFile>();
        _documentService.Setup(x => x.Upload(It.IsAny<DocumentDto>(), _cancellationToken))
            .ReturnsAsync(document);

        // Act
        var result = await _controller.Upload(document.Name, file.Object, _cancellationToken);

        // Assert
        var okResult = AssertOkResult<DocumentDto>(result.Result);
        AssertDocumentEquals(okResult, document);
    }

    [Test]
    public async Task Get_ValidId_ReturnsOkWithDocumentDto()
    {
        // Arrange 
        var document = CreateTestDocument();
        _documentService.Setup(x => x.GetDocument(document.Id, _cancellationToken))
            .ReturnsAsync(document);

        // Act
        var result = await _controller.Get(document.Id, _cancellationToken);

        // Assert
        var okResult = AssertOkResult<DocumentDto>(result.Result);
        AssertDocumentEquals(okResult, document);
    }

    [Test]
    public async Task GetAll_ValidRequest_ReturnsOkWithDocumentList()
    {
        // Arrange
        var documents = new List<DocumentDto>
        {
            CreateTestDocument(1, "test1.pdf"),
            CreateTestDocument(2, "test2.pdf")
        };

        _documentService.Setup(x => x.GetAllDocuments(_cancellationToken))
            .ReturnsAsync(documents);

        // Act
        var result = await _controller.GetAll(_cancellationToken);

        // Assert
        var okResult = AssertOkResult<List<DocumentDto>>(result.Result);
        Assert.That(okResult.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task Download_ValidId_ReturnsFileStream()
    {
        // Arrange
        var document = CreateTestDocument();
        var fileStream = new MemoryStream();

        _documentService.Setup(x => x.GetDocument(document.Id, _cancellationToken))
            .ReturnsAsync(document);
        _storageService.Setup(x => x.GetFileAsync(document.FilePath, _cancellationToken))
            .ReturnsAsync(fileStream);

        // Act
        var result = await _controller.Download(document.Id, _cancellationToken);

        // Assert
        Assert.That(result, Is.TypeOf<FileStreamResult>());
        var fileResult = result as FileStreamResult;
        Assert.That(fileResult?.FileDownloadName, Is.EqualTo(document.Name));
    }

    [Test]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        var documentId = 1;

        // Act
        var result = await _controller.Delete(documentId, _cancellationToken);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
        _documentService.Verify(x => x.DeleteDocument(documentId, _cancellationToken), Times.Once);
    }

    private static DocumentDto CreateTestDocument(int id = 1, string name = "test.pdf")
    {
        return new DocumentDto 
        { 
            Id = id, 
            Name = name,
            FilePath = $"path/{name}"
        };
    }

    private static T AssertOkResult<T>(IActionResult? result)
    {
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var value = (result as OkObjectResult)?.Value;
        Assert.That(value, Is.TypeOf<T>());
        return (T)value!;
    }

    private static void AssertDocumentEquals(DocumentDto actual, DocumentDto expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.Name, Is.EqualTo(expected.Name));
    }
}