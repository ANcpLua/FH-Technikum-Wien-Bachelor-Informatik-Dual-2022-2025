using DAL.Infrastructure;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.DAL;

[TestFixture]
public class TourRepositoryTests
{
    private TourPlannerContext _context;
    private TourRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TourPlannerContext>()
            .UseInMemoryDatabase(databaseName: $"TourPlannerTestDb_{Guid.NewGuid()}")
            .Options;
        _context = new TourPlannerContext(options);
        _repository = new TourRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task CreateTourAsync_WithValidTour_ReturnsSavedTour()
    {
        // Arrange
        var tour = TestData.CreateSampleTourPersistence();

        // Act
        var result = await _repository.CreateTourAsync(tour);
        var tourCount = await _context.ToursPersistence.CountAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(tour.Id));
            Assert.That(result.Name, Is.EqualTo(tour.Name));
            Assert.That(tourCount, Is.EqualTo(1));
        });
    }

    [Test]
    public void CreateTourAsync_WithInvalidTour_ThrowsDbUpdateException()
    {
        // Arrange
        var tour = TestData.CreateSampleTourPersistence();
        tour.Name = null!;

        // Act & Assert
        Assert.That (async () => await _repository.CreateTourAsync(tour), Throws.InstanceOf<DbUpdateException>());
    }

    [Test]
    public void GetAllTours_WithExistingTours_ReturnsAllTours()
    {
        // Arrange
        var tours = TestData.CreateSampleTourPersistenceList();
         _context.ToursPersistence.AddRange(tours);
         _context.SaveChanges();

        // Act
        var result = _repository.GetAllTours().ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(tours.Count));
    }

    [Test]
    public void GetAllTours_WithNoExistingTours_ReturnsEmptyList()
    {
        // Act
        var result = _repository.GetAllTours().ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetTourById_WithExistingId_ReturnsTour()
    {
        // Arrange
        var tour = TestData.CreateSampleTourPersistence();
        _context.ToursPersistence.Add(tour);
        _context.SaveChanges();

        // Act
        var result = _repository.GetTourById(tour.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(tour.Id));
            Assert.That(result.Name, Is.EqualTo(tour.Name));
        });
    }

    [Test]
    public void GetTourById_WithNonExistingId_ReturnsNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = _repository.GetTourById(nonExistingId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateTourAsync_WithExistingTour_ReturnsUpdatedTour()
    {
        // Arrange
        var tour = TestData.CreateSampleTourPersistence();
        await _context.ToursPersistence.AddAsync(tour);
        await _context.SaveChangesAsync();

        tour.Name = "Updated Tour Name";

        // Act
        var result = await _repository.UpdateTourAsync(tour);
        var dbTour = await _context.ToursPersistence.FirstAsync(t => t.Id == tour.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo("Updated Tour Name"));
            Assert.That(dbTour.Name, Is.EqualTo("Updated Tour Name"));
        });
    }

    [Test]
    public Task UpdateTourAsync_WithNonExistingTour_ThrowsDbUpdateConcurrencyException()
    {
        // Arrange
        var tour = TestData.CreateSampleTourPersistence();

        // Act & Assert
        Assert.That(async () => await _repository.UpdateTourAsync(tour), Throws.InstanceOf<DbUpdateConcurrencyException>());
        return Task.CompletedTask;
    }

    [Test]
    public async Task DeleteTourAsync_WithExistingId_RemovesTourFromDatabase()
    {
        // Arrange
        var tour = TestData.CreateSampleTourPersistence();
        _context.ToursPersistence.Add(tour);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteTourAsync(tour.Id);

        // Assert
        Assert.That(await _context.ToursPersistence.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public async Task DeleteTourAsync_WithNonExistingId_DoesNotRemoveAnyTourFromDatabase()
    {
        // Arrange
        var tour = TestData.CreateSampleTourPersistence();
        _context.ToursPersistence.Add(tour);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteTourAsync(Guid.NewGuid());

        // Assert
        Assert.That(await _context.ToursPersistence.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task SearchToursAsync_WithMatchingSearchText_ReturnsMatchingTours()
    {
        // Arrange
        var tours = TestData.CreateSampleTourPersistenceList();
        await _context.ToursPersistence.AddRangeAsync(tours);
        await _context.SaveChangesAsync();

        // Act
        var result = _repository.SearchToursAsync("Sample");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Sample Tour"));
        });
    }

    [Test]
    public async Task SearchToursAsync_WithNonMatchingSearchText_ReturnsEmptyList()
    {
        // Arrange
        var tours = TestData.CreateSampleTourPersistenceList();
        await _context.ToursPersistence.AddRangeAsync(tours);
        await _context.SaveChangesAsync();

        // Act
        var result = _repository.SearchToursAsync("NonExistingTour");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(0));
    }
}