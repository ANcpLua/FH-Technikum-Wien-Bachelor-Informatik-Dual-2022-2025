using AutoMapper;
using Contract;
using Microsoft.AspNetCore.Http;
using Moq;
using PaperlessServices.AutoMapper;
using PaperlessServices.Entities;
using PostgreSQL.Entities;

namespace Tests.Mapper;

[TestFixture]
public class MappingTests
{
    private IMapper _mapper;
    private MapperConfiguration _configuration;

    [OneTimeSetUp]
    public void Initialize()
    {
        _configuration = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperConfig>(); });
        _mapper = _configuration.CreateMapper();
    }

    #region Configuration Validation

    [Test]
    public void Configuration_IsValid()
    {
        // Assert
        _configuration.AssertConfigurationIsValid();
    }

    #endregion

    #region BlDocument to DocumentDto

    [Test]
    public void Map_BlDocumentToDocumentDto_MapsCorrectly()
    {
        // Arrange
        var source = new BlDocument
        {
            Id = 1,
            Name = "Test Document",
            FilePath = "/test/path.pdf",
            DateUploaded = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            File = Mock.Of<IFormFile>(),
            OcrText = "Sample OCR Text"
        };

        // Act
        var result = _mapper.Map<DocumentDto>(source);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(source.Id));
            Assert.That(result.Name, Is.EqualTo(source.Name));
            Assert.That(result.FilePath, Is.EqualTo(source.FilePath));
            Assert.That(result.DateUploaded, Is.EqualTo(source.DateUploaded));
            Assert.That(result.File, Is.Null); // Ignored
            Assert.That(result.OcrText, Is.EqualTo(source.OcrText));
        });
    }

    [Test]
    public void Map_BlDocumentToDocumentDto_WithNullOcrText_MapsCorrectly()
    {
        // Arrange
        var source = new BlDocument
        {
            Id = 2,
            Name = "Document Without OCR",
            FilePath = "/path/without/ocr.pdf",
            DateUploaded = DateTime.UtcNow,
            OcrText = null
        };

        // Act
        var result = _mapper.Map<DocumentDto>(source);

        // Assert
        Assert.Multiple(() => { Assert.That(result.OcrText, Is.Null); });
    }

    [Test]
    public void Map_NullBlDocumentToDocumentDto_ReturnsNull()
    {
        // Act
        var result = _mapper.Map<DocumentDto>(null);

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region DocumentDto to BlDocument

    [Test]
    public void Map_DocumentDtoToBlDocument_MapsCorrectly()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        var source = new DocumentDto
        {
            Id = 1,
            Name = "Test Document",
            FilePath = "/test/path.pdf",
            DateUploaded = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            File = mockFile.Object,
            OcrText = "Sample OCR Text"
        };

        // Act
        var result = _mapper.Map<BlDocument>(source);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(source.Id));
            Assert.That(result.Name, Is.EqualTo(source.Name));
            Assert.That(result.FilePath, Is.EqualTo(source.FilePath));
            Assert.That(result.DateUploaded, Is.EqualTo(source.DateUploaded));
            Assert.That(result.File, Is.EqualTo(source.File));
            Assert.That(result.OcrText, Is.EqualTo(source.OcrText));
        });
    }

    [Test]
    public void Map_DocumentDtoToBlDocument_WithEmptyFilePath_SetsFilePathToNull()
    {
        // Arrange
        var source = new DocumentDto
        {
            Name = "Document with Empty FilePath",
            FilePath = string.Empty
        };

        // Act
        var result = _mapper.Map<BlDocument>(source);

        // Assert
        Assert.That(result.FilePath, Is.Null);
    }

    [Test]
    public void Map_DocumentDtoToBlDocument_WithDefaultDateUploaded_SetsToUtcNow()
    {
        // Arrange
        var source = new DocumentDto
        {
            Name = "Document with Default Date",
            DateUploaded = default
        };

        // Act
        var beforeMapping = DateTime.UtcNow;
        var result = _mapper.Map<BlDocument>(source);
        var afterMapping = DateTime.UtcNow;

        // Assert
        Assert.That(result.DateUploaded, Is.InRange(beforeMapping, afterMapping));
    }

    [Test]
    public void Map_NullDocumentDtoToBlDocument_ReturnsNull()
    {
        // Act
        var result = _mapper.Map<BlDocument>(null);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Map_PartialDocumentDtoToBlDocument_SetsDefaults()
    {
        // Arrange
        var source = new DocumentDto
        {
            Name = "Partial Document"
            // FilePath and DateUploaded are missing
        };

        // Act
        var result = _mapper.Map<BlDocument>(source);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(source.Name));
            Assert.That(result.FilePath, Is.Null); // Default for missing FilePath
            Assert.That(result.DateUploaded, Is.Not.EqualTo(default(DateTime))); // Defaults to current UTC date
            Assert.That(result.File, Is.Null); // Default
            Assert.That(result.OcrText, Is.Null); // Default
        });
    }

    #endregion

    #region Document to BlDocument

    [Test]
    public void Map_DocumentToBlDocument_MapsCorrectly()
    {
        // Arrange
        var source = new Document
        {
            Id = 1,
            Name = "Test Document",
            FilePath = "/test/path.pdf",
            DateUploaded = DateTime.UtcNow,
            Content = "Sample Content",
            OcrText = "Sample OCR Text"
        };

        // Act
        var result = _mapper.Map<BlDocument>(source);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(source.Id));
            Assert.That(result.Name, Is.EqualTo(source.Name));
            Assert.That(result.FilePath, Is.EqualTo(source.FilePath));
            Assert.That(result.DateUploaded, Is.EqualTo(source.DateUploaded));
            Assert.That(result.Content, Is.EqualTo(source.Content));
            Assert.That(result.OcrText, Is.EqualTo(source.OcrText));
            Assert.That(result.File, Is.Null); // Ignored
        });
    }

    [Test]
    public void Map_DocumentToBlDocument_WithNullOcrText_SetsOcrTextToEmptyString()
    {
        // Arrange
        var source = new Document
        {
            Id = 2,
            Name = "Document Without OCR",
            FilePath = "/path/without/ocr.pdf",
            DateUploaded = DateTime.UtcNow,
            OcrText = null
        };

        // Act
        var result = _mapper.Map<BlDocument>(source);

        // Assert
        Assert.That(result.OcrText, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Map_NullDocumentToBlDocument_ReturnsNull()
    {
        // Act
        var result = _mapper.Map<BlDocument>(null);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Map_DocumentListToBlDocumentList_MapsCorrectly()
    {
        // Arrange
        var sourceList = new List<Document>
        {
            new Document
            {
                Id = 1, Name = "Doc1", FilePath = "/path1.pdf", DateUploaded = DateTime.UtcNow, OcrText = "OCR1"
            },
            new Document
                { Id = 2, Name = "Doc2", FilePath = "/path2.pdf", DateUploaded = DateTime.UtcNow, OcrText = null }
        };

        // Act
        var resultList = _mapper.Map<List<BlDocument>>(sourceList);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultList.Count, Is.EqualTo(sourceList.Count));
            Assert.That(resultList[0].Name, Is.EqualTo(sourceList[0].Name));
            Assert.That(resultList[0].OcrText, Is.EqualTo(sourceList[0].OcrText));
            Assert.That(resultList[1].OcrText, Is.EqualTo(string.Empty)); // Mapped to empty string
        });
    }

    #endregion

    #region BlDocument to Document

    [Test]
    public void Map_BlDocumentToDocument_MapsCorrectly()
    {
        // Arrange
        var source = new BlDocument
        {
            Id = 1,
            Name = "Test Document",
            FilePath = "/test/path.pdf",
            DateUploaded = DateTime.UtcNow,
            Content = "Sample Content",
            OcrText = "Sample OCR Text",
            File = Mock.Of<IFormFile>()
        };

        // Act
        var result = _mapper.Map<Document>(source);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(source.Id));
            Assert.That(result.Name, Is.EqualTo(source.Name));
            Assert.That(result.FilePath, Is.EqualTo(source.FilePath));
            Assert.That(result.DateUploaded, Is.EqualTo(source.DateUploaded));
            Assert.That(result.Content, Is.EqualTo(source.Content));
            Assert.That(result.OcrText, Is.EqualTo(source.OcrText));
            // Content is mapped, File is ignored in Document
        });
    }

    [Test]
    public void Map_BlDocumentToDocument_WithNullOcrText_SetsOcrTextToEmptyString()
    {
        // Arrange
        var source = new BlDocument
        {
            Id = 2,
            Name = "Document Without OCR",
            FilePath = "/path/without/ocr.pdf",
            DateUploaded = DateTime.UtcNow,
            Content = "Content without OCR",
            OcrText = null
        };

        // Act
        var result = _mapper.Map<Document>(source);

        // Assert
        Assert.That(result.OcrText, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Map_BlDocumentToDocument_WithNonZeroId_MapsId()
    {
        // Arrange
        var source = new BlDocument
        {
            Id = 42,
            Name = "Test Document",
            FilePath = "/test/path.pdf",
            DateUploaded = DateTime.UtcNow,
            Content = "Sample Content",
            OcrText = "Sample OCR Text"
        };

        // Act
        var result = _mapper.Map<Document>(source);

        // Assert
        Assert.That(result.Id, Is.EqualTo(source.Id));
    }

    [Test]
    public void Map_NullBlDocumentToDocument_ReturnsNull()
    {
        // Act
        var result = _mapper.Map<Document>(null);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Map_BlDocumentToDocument_WithLongFilePath_MapsCorrectly()
    {
        // Arrange
        var longFilePath = new string('a', 500); // 500 characters
        var source = new BlDocument
        {
            Id = 3,
            Name = "Long Path Document",
            FilePath = longFilePath,
            DateUploaded = DateTime.UtcNow,
            Content = "Content with long path",
            OcrText = "OCR Text"
        };

        // Act
        var result = _mapper.Map<Document>(source);

        // Assert
        Assert.That(result.FilePath, Is.EqualTo(longFilePath));
    }

    [Test]
    public void Map_BlDocumentToDocumentList_MapsCorrectly()
    {
        // Arrange
        var sourceList = new List<BlDocument>
        {
            new BlDocument
            {
                Id = 1, Name = "Doc1", FilePath = "/path1.pdf", DateUploaded = DateTime.UtcNow, OcrText = "OCR1"
            },
            new BlDocument
                { Id = 2, Name = "Doc2", FilePath = "/path2.pdf", DateUploaded = DateTime.UtcNow, OcrText = null }
        };

        // Act
        var resultList = _mapper.Map<List<Document>>(sourceList);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultList.Count, Is.EqualTo(sourceList.Count));
            Assert.That(resultList[0].Name, Is.EqualTo(sourceList[0].Name));
            Assert.That(resultList[0].OcrText, Is.EqualTo(sourceList[0].OcrText));
            Assert.That(resultList[1].OcrText, Is.EqualTo(string.Empty)); // Mapped to empty string
        });
    }

    #endregion

    #region Document to DocumentDto

    [Test]
    public void Map_NullDocumentToDocumentDto_ReturnsNull()
    {
        // Act
        var result = _mapper.Map<DocumentDto>(null);

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region Additional Edge Cases

    [Test]
    public void Map_DocumentWithInvalidDateUploaded_MapsCorrectly()
    {
        // Arrange
        var source = new Document
        {
            Id = 3,
            Name = "Invalid Date Document",
            FilePath = "/invalid/date.pdf",
            DateUploaded = DateTime.MinValue,
            OcrText = "Invalid Date OCR"
        };

        // Act
        var result = _mapper.Map<BlDocument>(source);

        // Assert
        Assert.That(result.DateUploaded, Is.EqualTo(DateTime.MinValue));
    }

    [Test]
    public void Map_BlDocumentToDocument_WithFutureDateUploaded_MapsCorrectly()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(10);
        var source = new BlDocument
        {
            Id = 4,
            Name = "Future Date Document",
            FilePath = "/future/date.pdf",
            DateUploaded = futureDate,
            OcrText = "Future OCR Text"
        };

        // Act
        var result = _mapper.Map<Document>(source);

        // Assert
        Assert.That(result.DateUploaded, Is.EqualTo(futureDate));
    }

    [Test]
    public void Map_DocumentDtoToBlDocument_WithNullFile_MapsCorrectly()
    {
        // Arrange
        var source = new DocumentDto
        {
            Id = 5,
            Name = "Document Without File",
            FilePath = "/no/file.pdf",
            DateUploaded = DateTime.UtcNow,
            File = null,
            OcrText = "No File OCR"
        };

        // Act
        var result = _mapper.Map<BlDocument>(source);

        // Assert
        Assert.That(result.File, Is.Null);
    }

    #endregion
}
