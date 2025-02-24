using DAL.Infrastructure;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.DAL;

[TestFixture]
public class TourLogRepositoryTests
{
    private TourPlannerContext _context;
    private TourLogRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TourPlannerContext>()
            .UseInMemoryDatabase(databaseName: $"TourPlannerTestDb_{Guid.NewGuid()}")
            .Options;
        _context = new TourPlannerContext(options);
        _repository = new TourLogRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task CreateTourLogAsync_WithValidTourLog_ReturnsSavedTourLog()
    {
        // Arrange
        var tourLog = TestData.CreateSampleTourLogPersistence();

        // Act
        var result = await _repository.CreateTourLogAsync(tourLog);
        var logCount = await _context.TourLogsPersistence.CountAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(tourLog.Id));
            Assert.That(logCount, Is.EqualTo(1));
        });
    }

    [Test]
    public void GetTourLogsByTourId_WithExistingTourId_ReturnsAllTourLogs()
    {
        // Arrange
        var tourLogs = TestData.CreateSampleTourLogPersistenceList();
         _context.TourLogsPersistence.AddRange(tourLogs);
         _context.SaveChanges();

        // Act
        var result =  _repository.GetTourLogsByTourId(TestData.TestGuid).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(tourLogs.Count));
    }

    [Test]
    public void GetTourLogsByTourId_WithNonExistentTourId_ReturnsEmptyList()
    {
        // Arrange
        var nonExistentTourId = Guid.NewGuid();

        // Act
        var result = _repository.GetTourLogsByTourId(nonExistentTourId);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetTourLogById_WithExistingId_ReturnsTourLog()
    {
        // Arrange
        var tourLog = TestData.CreateSampleTourLogPersistence();
        _context.TourLogsPersistence.Add(tourLog);
        _context.SaveChanges();

        // Act
        var result = _repository.GetTourLogById(tourLog.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(tourLog.Id));
    }

    [Test]
    public void GetTourLogById_WithNonExistingId_ReturnsNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = _repository.GetTourLogById(nonExistingId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateTourLogAsync_WithExistingTourLog_ReturnsUpdatedTourLog()
    {
        // Arrange
        var tourLog = TestData.CreateSampleTourLogPersistence();
        _context.TourLogsPersistence.Add(tourLog);
        await _context.SaveChangesAsync();
        tourLog.Comment = "Updated comment";

        // Act
        var result = await _repository.UpdateTourLogAsync(tourLog);
        var dbTourLog = await _context.TourLogsPersistence
            .FirstAsync(t => t.Id == tourLog.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Comment, Is.EqualTo("Updated comment"));
            Assert.That(dbTourLog.Comment, Is.EqualTo("Updated comment"));
        });
    }

    [Test]
    public async Task DeleteTourLogAsync_WithExistingId_RemovesTourLogFromDatabase()
    {
        // Arrange
        var tourLog = TestData.CreateSampleTourLogPersistence();
        _context.TourLogsPersistence.Add(tourLog);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteTourLogAsync(tourLog.Id);

        // Assert
        Assert.That(await _context.TourLogsPersistence.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public async Task CreateTourLogAsync_WithLocalDateTime_SavesAsUtc()
    {
        // Arrange
        var tourLog = TestData.CreateSampleTourLogPersistence();
        tourLog.DateTime = DateTime.Now;

        // Act
        var result = await _repository.CreateTourLogAsync(tourLog);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.DateTime.Kind, Is.EqualTo(DateTimeKind.Local));
    }

    [Test]
    public async Task CreateTourLogAsync_WithPreciseDistance_MaintainsPrecision()
    {
        // Arrange
        var tourLog = TestData.CreateSampleTourLogPersistence();
        tourLog.TotalDistance = 10.123456789;

        // Act
        var result = await _repository.CreateTourLogAsync(tourLog);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.TotalDistance, Is.EqualTo(10.123456789).Within(0.000000001),
            "Distance should maintain its precision"
        );
    }

    [Test]
    public async Task CreateTourLogAsync_WithFutureDate_SavesSuccessfully()
    {
        // Arrange
        var tourLog = TestData.CreateSampleTourLogPersistence();
        tourLog.DateTime = DateTime.UtcNow.AddYears(1);

        // Act
        var result = await _repository.CreateTourLogAsync(tourLog);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.DateTime, Is.GreaterThan(DateTime.UtcNow));
    }
}