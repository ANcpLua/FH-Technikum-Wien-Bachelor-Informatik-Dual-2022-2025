using BL.DomainModel;
using BL.Service;
using DAL.Interface;
using DAL.PersistenceModel;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Test;

[TestFixture]
public class TourLogServiceTests
{
    [SetUp]
    public void Setup()
    {
        _mockTourLogRepository = new Mock<ITourLogRepository>();
        _mockMapper = new Mock<IMapper>();
        _tourLogService = new TourLogService(_mockTourLogRepository.Object, _mockMapper.Object);
    }

    private Mock<ITourLogRepository> _mockTourLogRepository;
    private Mock<IMapper> _mockMapper;
    private TourLogService _tourLogService;

    [Test]
    public async Task CreateTourLogAsync_ValidTourLog_ReturnsMappedTourLogDomain()
    {
        // Arrange
        var tourLogDomain = TestData.CreateSampleTourLogDomainList().First();
        var tourLogPersistence = TestData.CreateSampleTourLogPersistence();
        _mockMapper
            .Setup(m => m.Map<TourLogPersistence>(tourLogDomain))
            .Returns(tourLogPersistence);
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogPersistence)).Returns(tourLogDomain);
        _mockTourLogRepository
            .Setup(r => r.CreateTourLogAsync(tourLogPersistence, default))
            .ReturnsAsync(tourLogPersistence);

        // Act
        var result = await _tourLogService.CreateTourLogAsync(tourLogDomain);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result.Id, Is.EqualTo(tourLogDomain.Id));
            Assert.That(result.Comment, Is.EqualTo(tourLogDomain.Comment));
            Assert.That(result.DateTime, Is.EqualTo(tourLogDomain.DateTime));
        });
        _mockTourLogRepository.Verify(
        r => r.CreateTourLogAsync(tourLogPersistence, default),
        Times.Once
        );
    }

    [Test]
    public Task CreateTourLogAsync_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var tourLogDomain = TestData.CreateSampleTourLogDomainList().First();
        var tourLogPersistence = TestData.CreateSampleTourLogPersistence();
        _mockMapper
            .Setup(m => m.Map<TourLogPersistence>(tourLogDomain))
            .Returns(tourLogPersistence);
        _mockTourLogRepository
            .Setup(r => r.CreateTourLogAsync(tourLogPersistence, default))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(
        async () => await _tourLogService.CreateTourLogAsync(tourLogDomain)
        );
        Assert.That(ex.Message, Is.EqualTo("Database error"));
        return Task.CompletedTask;
    }

    [Test]
    public void GetTourLogById_ExistingId_ReturnsMappedTourLogDomain()
    {
        // Arrange
        var tourLogPersistence = TestData.CreateSampleTourLogPersistence();
        var tourLogDomain = TestData.CreateSampleTourLogDomainList().First();
        _mockTourLogRepository
            .Setup(r => r.GetTourLogById(tourLogPersistence.Id))
            .Returns(tourLogPersistence);
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogPersistence)).Returns(tourLogDomain);

        // Act
        var result = _tourLogService.GetTourLogById(tourLogPersistence.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(tourLogPersistence.Id));
        _mockTourLogRepository.Verify(r => r.GetTourLogById(tourLogPersistence.Id), Times.Once);
    }

    [Test]
    public void GetTourLogById_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockTourLogRepository
            .Setup(r => r.GetTourLogById(TestData.NonexistentGuid))
            .Returns((TourLogPersistence)null!);

        // Act
        var result = _tourLogService.GetTourLogById(TestData.NonexistentGuid);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetTourLogsByTourIdAsync_ExistingTourId_ReturnsAllMappedTourLogs()
    {
        // Arrange
        var tourLogsPersistence = TestData.CreateSampleTourLogPersistenceList();
        var tourLogsDomain = TestData.CreateSampleTourLogDomainList();
        _mockTourLogRepository
            .Setup(r => r.GetTourLogsByTourIdAsync(TestData.TestGuid, default))
            .ReturnsAsync(tourLogsPersistence);
        _mockMapper
            .Setup(m => m.Map<IEnumerable<TourLogDomain>>(tourLogsPersistence))
            .Returns(tourLogsDomain);

        // Act
        var result = (await _tourLogService.GetTourLogsByTourIdAsync(TestData.TestGuid)).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(tourLogsDomain.Count));
        _mockTourLogRepository.Verify(
        r => r.GetTourLogsByTourIdAsync(TestData.TestGuid, default),
        Times.Once
        );
    }

    [Test]
    public async Task GetTourLogsByTourIdAsync_NonExistingTourId_ReturnsEmptyList()
    {
        // Arrange
        _mockTourLogRepository
            .Setup(r => r.GetTourLogsByTourIdAsync(TestData.NonexistentGuid, default))
            .ReturnsAsync(new List<TourLogPersistence>());
        _mockMapper
            .Setup(m =>
                m.Map<IEnumerable<TourLogDomain>>(It.IsAny<IEnumerable<TourLogPersistence>>())
            )
            .Returns(new List<TourLogDomain>());

        // Act
        var result = await _tourLogService.GetTourLogsByTourIdAsync(TestData.NonexistentGuid);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task UpdateTourLogAsync_ExistingTourLog_ReturnsUpdatedMappedTourLogDomain()
    {
        // Arrange
        var tourLogDomain = TestData.CreateSampleTourLogDomainList().First();
        var tourLogPersistence = TestData.CreateSampleTourLogPersistence();
        _mockMapper
            .Setup(m => m.Map<TourLogPersistence>(tourLogDomain))
            .Returns(tourLogPersistence);
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogPersistence)).Returns(tourLogDomain);
        _mockTourLogRepository
            .Setup(r => r.UpdateTourLogAsync(tourLogPersistence, default))
            .ReturnsAsync(tourLogPersistence);

        // Act
        var result = await _tourLogService.UpdateTourLogAsync(tourLogDomain);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result.Id, Is.EqualTo(tourLogDomain.Id));
            Assert.That(result.Comment, Is.EqualTo(tourLogDomain.Comment));
            Assert.That(result.DateTime, Is.EqualTo(tourLogDomain.DateTime));
        });
        _mockTourLogRepository.Verify(
        r => r.UpdateTourLogAsync(tourLogPersistence, default),
        Times.Once
        );
    }

    [Test]
    public Task UpdateTourLogAsync_NonExistingTourLog_ThrowsException()
    {
        // Arrange
        var tourLogDomain = TestData.CreateSampleTourLogDomainList().First();
        var tourLogPersistence = TestData.CreateSampleTourLogPersistence();
        _mockMapper
            .Setup(m => m.Map<TourLogPersistence>(tourLogDomain))
            .Returns(tourLogPersistence);
        _mockTourLogRepository
            .Setup(r => r.UpdateTourLogAsync(tourLogPersistence, default))
            .ThrowsAsync(new InvalidOperationException("Tour log not found"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
        async () => await _tourLogService.UpdateTourLogAsync(tourLogDomain)
        );
        Assert.That(ex.Message, Is.EqualTo("Tour log not found"));
        return Task.CompletedTask;
    }

    [Test]
    public async Task DeleteTourLogAsync_ExistingId_CallsRepositoryDelete()
    {
        // Arrange
        var tourLogId = TestData.CreateSampleTourLogPersistence().Id;
        _mockTourLogRepository
            .Setup(r => r.DeleteTourLogAsync(tourLogId, default))
            .Returns(Task.CompletedTask);

        // Act
        await _tourLogService.DeleteTourLogAsync(tourLogId);

        // Assert
        _mockTourLogRepository.Verify(r => r.DeleteTourLogAsync(tourLogId, default), Times.Once);
    }

    [Test]
    public Task DeleteTourLogAsync_NonExistingId_DoesNotThrowException()
    {
        // Arrange
        _mockTourLogRepository
            .Setup(r => r.DeleteTourLogAsync(TestData.NonexistentGuid, default))
            .Returns(Task.CompletedTask);

        // Act & Assert
        Assert.DoesNotThrowAsync(
        async () => await _tourLogService.DeleteTourLogAsync(TestData.NonexistentGuid)
        );
        return Task.CompletedTask;
    }

    [Test]
    public Task CreateTourLogAsync_CancellationRequested_ThrowsOperationCanceledException()
    {
        // Arrange
        var tourLogDomain = TestData.CreateSampleTourLogDomainList().First();
        var tourLogPersistence = TestData.CreateSampleTourLogPersistence();
        _mockMapper
            .Setup(m => m.Map<TourLogPersistence>(tourLogDomain))
            .Returns(tourLogPersistence);
        _mockTourLogRepository
            .Setup(r => r.CreateTourLogAsync(tourLogPersistence, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        Assert.ThrowsAsync<OperationCanceledException>(
        async () => await _tourLogService.CreateTourLogAsync(tourLogDomain, cts.Token)
        );
        return Task.CompletedTask;
    }

    [Test]
    public async Task GetTourLogsByTourIdAsync_LargeTourLogCount_HandlesLargeDataSet()
    {
        // Arrange
        var largeTourLogList = Enumerable
            .Range(0, 10000)
            .Select(_ => TestData.CreateSampleTourLogPersistence())
            .ToList();

        var largeTourLogDomainList = Enumerable
            .Range(0, 10000)
            .Select(_ => TestData.CreateSampleTourLogDomainList().First())
            .ToList();

        _mockTourLogRepository
            .Setup(r => r.GetTourLogsByTourIdAsync(TestData.TestGuid, default))
            .ReturnsAsync(largeTourLogList);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<TourLogDomain>>(largeTourLogList))
            .Returns(largeTourLogDomainList);

        // Act
        var result = (await _tourLogService.GetTourLogsByTourIdAsync(TestData.TestGuid)).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(largeTourLogDomainList.Count));
        _mockTourLogRepository.Verify(
        r => r.GetTourLogsByTourIdAsync(TestData.TestGuid, default),
        Times.Once
        );
    }

    [Test]
    public async Task UpdateTourLogAsync_ConcurrentUpdates_HandlesRaceCondition()
    {
        // Arrange
        var tourLogDomain = TestData.CreateSampleTourLogDomainList().First();
        var tourLogPersistence = TestData.CreateSampleTourLogPersistence();
        _mockMapper
            .Setup(m => m.Map<TourLogPersistence>(tourLogDomain))
            .Returns(tourLogPersistence);
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogPersistence)).Returns(tourLogDomain);

        _mockTourLogRepository
            .SetupSequence(r => r.UpdateTourLogAsync(tourLogPersistence, default))
            .ThrowsAsync(new DbUpdateConcurrencyException("Update conflict"))
            .ReturnsAsync(tourLogPersistence);

        // Act & Assert
        Assert.ThrowsAsync<DbUpdateConcurrencyException>(
        async () => await _tourLogService.UpdateTourLogAsync(tourLogDomain)
        );
        var result = await _tourLogService.UpdateTourLogAsync(tourLogDomain);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(tourLogDomain.Id));
    }
}
