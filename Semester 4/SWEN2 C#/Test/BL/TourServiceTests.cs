using BL.DomainModel;
using BL.Service;
using DAL.Interface;
using DAL.PersistenceModel;
using MapsterMapper;
using Moq;

namespace Test.BL;

[TestFixture]
public class TourServiceTests
{
    private Mock<ITourRepository> _mockTourRepository;
    private Mock<IMapper> _mockMapper;
    private TourService _tourService;

    [SetUp]
    public void Setup()
    {
        _mockTourRepository = new Mock<ITourRepository>();
        _mockMapper = new Mock<IMapper>();
        _tourService = new TourService(_mockTourRepository.Object, _mockMapper.Object);
    }

    [Test]
    public async Task CreateTourAsync_ValidTour_ReturnsMappedTourDomain()
    {
        // Arrange
        var tourDomain = TestData.CreateSampleTourDomainList().First();
        var tourPersistence = TestData.CreateSampleTourPersistence();
        _mockMapper.Setup(m => m.Map<TourPersistence>(tourDomain)).Returns(tourPersistence);
        _mockMapper.Setup(m => m.Map<TourDomain>(tourPersistence)).Returns(tourDomain);
        _mockTourRepository
            .Setup(r => r.CreateTourAsync(tourPersistence))
            .ReturnsAsync(tourPersistence);

        // Act
        var result = await _tourService.CreateTourAsync(tourDomain);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(tourDomain.Id));
            Assert.That(result.Name, Is.EqualTo(tourDomain.Name));
            Assert.That(result.Description, Is.EqualTo(tourDomain.Description));
        });
        _mockTourRepository.Verify(r => r.CreateTourAsync(tourPersistence), Times.Once);
    }

    [Test]
    public Task CreateTourAsync_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var tourDomain = TestData.CreateSampleTourDomainList().First();
        var tourPersistence = TestData.CreateSampleTourPersistence();
        _mockMapper.Setup(m => m.Map<TourPersistence>(tourDomain)).Returns(tourPersistence);
        _mockTourRepository
            .Setup(r => r.CreateTourAsync(tourPersistence))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(
            async () => await _tourService.CreateTourAsync(tourDomain)
        );
        Assert.That(ex.Message, Is.EqualTo("Database error"));
        return Task.CompletedTask;
    }

    [Test]
    public void GetAllToursAsync_ToursExist_ReturnsAllMappedTours()
    {
        // Arrange
        var toursPersistence = TestData.CreateSampleTourPersistenceList();
        var toursDomain = TestData.CreateSampleTourDomainList();
        _mockTourRepository.Setup(r => r.GetAllTours()).Returns(toursPersistence);
        _mockMapper
            .Setup(m => m.Map<IEnumerable<TourDomain>>(toursPersistence))
            .Returns(toursDomain);

        // Act
        var result =  _tourService.GetAllTours().ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(toursDomain.Count));
        _mockTourRepository.Verify(r => r.GetAllTours(), Times.Once);
    }

    [Test]
    public void GetAllToursAsync_NoToursExist_ReturnsEmptyList()
    {
        // Arrange
        _mockTourRepository
            .Setup(r => r.GetAllTours())
            .Returns(new List<TourPersistence>());
        _mockMapper
            .Setup(m => m.Map<IEnumerable<TourDomain>>(It.IsAny<IEnumerable<TourPersistence>>()))
            .Returns(new List<TourDomain>());

        // Act
        var result =  _tourService.GetAllTours();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetTourById_ExistingId_ReturnsMappedTourDomain()
    {
        // Arrange
        var tourPersistence = TestData.CreateSampleTourPersistence();
        var tourDomain = TestData.CreateSampleTourDomainList().First();
        _mockTourRepository.Setup(r => r.GetTourById(TestData.TestGuid)).Returns(tourPersistence);
        _mockMapper.Setup(m => m.Map<TourDomain>(tourPersistence)).Returns(tourDomain);

        // Act
        var result = _tourService.GetTourById(TestData.TestGuid);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(TestData.TestGuid));
        _mockTourRepository.Verify(r => r.GetTourById(TestData.TestGuid), Times.Once);
    }

    [Test]
    public void GetTourById_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockTourRepository
            .Setup(r => r.GetTourById(TestData.NonexistentGuid))
            .Returns((TourPersistence)null!);

        // Act
        var result = _tourService.GetTourById(TestData.NonexistentGuid);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateTourAsync_ExistingTour_ReturnsUpdatedMappedTourDomain()
    {
        // Arrange
        var tourDomain = TestData.CreateSampleTourDomainList().First();
        var tourPersistence = TestData.CreateSampleTourPersistence();
        _mockMapper.Setup(m => m.Map<TourPersistence>(tourDomain)).Returns(tourPersistence);
        _mockMapper.Setup(m => m.Map<TourDomain>(tourPersistence)).Returns(tourDomain);
        _mockTourRepository
            .Setup(r => r.UpdateTourAsync(tourPersistence))
            .ReturnsAsync(tourPersistence);

        // Act
        var result = await _tourService.UpdateTourAsync(tourDomain);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(tourDomain.Id));
        _mockTourRepository.Verify(r => r.UpdateTourAsync(tourPersistence), Times.Once);
    }

    [Test]
    public Task UpdateTourAsync_NonExistingTour_ThrowsException()
    {
        // Arrange
        var tourDomain = TestData.CreateSampleTourDomainList().First();
        var tourPersistence = TestData.CreateSampleTourPersistence();
        _mockMapper.Setup(m => m.Map<TourPersistence>(tourDomain)).Returns(tourPersistence);
        _mockTourRepository
            .Setup(r => r.UpdateTourAsync(tourPersistence))
            .ThrowsAsync(new InvalidOperationException("Tour not found"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _tourService.UpdateTourAsync(tourDomain)
        );
        Assert.That(ex.Message, Is.EqualTo("Tour not found"));
        return Task.CompletedTask;
    }

    [Test]
    public async Task DeleteTourAsync_ExistingId_CallsRepositoryDelete()
    {
        // Arrange
        _mockTourRepository
            .Setup(r => r.DeleteTourAsync(TestData.TestGuid))
            .Returns(Task.CompletedTask);

        // Act
        await _tourService.DeleteTourAsync(TestData.TestGuid);

        // Assert
        _mockTourRepository.Verify(r => r.DeleteTourAsync(TestData.TestGuid), Times.Once);
    }

    [Test]
    public void SearchTours_ValidSearchText_ReturnsFilteredMappedTours()
    {
        // Arrange
        var toursPersistence = TestData.CreateSampleTourPersistenceList().AsQueryable();
        var toursDomain = TestData.CreateSampleTourDomainList().AsQueryable();
        _mockTourRepository
            .Setup(r => r.SearchToursAsync(TestData.ValidSearchText))
            .Returns(toursPersistence);
        _mockMapper
            .Setup(m => m.Map<TourDomain>(It.IsAny<TourPersistence>()))
            .Returns<TourPersistence>(t => toursDomain.First(d => d.Id == t.Id));

        // Act
        var result = _tourService.SearchTours(TestData.ValidSearchText);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(toursDomain.Count()));
        _mockTourRepository.Verify(r => r.SearchToursAsync(TestData.ValidSearchText), Times.Once);
    }

    [Test]
    public void SearchTours_NoMatchingTours_ReturnsEmptyQueryable()
    {
        // Arrange
        _mockTourRepository
            .Setup(r => r.SearchToursAsync(TestData.InvalidSearchText))
            .Returns(new List<TourPersistence>().AsQueryable());

        // Act
        var result = _tourService.SearchTours(TestData.InvalidSearchText);

        // Assert
        Assert.That(result, Is.Empty);
    }
}