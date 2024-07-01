using BL.DomainModel;
using DAL.PersistenceModel;
using MapsterMapper;
using ORM;
using UI.Model;

namespace Test;

[TestFixture]
public class MappingConfigurationTests
{

    [SetUp]
    public void Setup()
    {
        var config = MappingConfiguration.RegisterMapping();
        _mapper = new Mapper(config);
    }
    private IMapper _mapper;

    [Test]
    public void TourPersistence_To_TourDomain_MapsTourLogsCorrectly()
    {
        var tourPersistence = TestData.CreateSampleTourPersistence();

        var tourDomain = _mapper.Map<TourDomain>(tourPersistence);

        Assert.That(tourDomain.Logs, Has.Count.EqualTo(tourPersistence.TourLogPersistence.Count));
    }

    [Test]
    public void TourDomain_To_TourPersistence_MapsTourLogsCorrectly()
    {
        var tourDomain = TestData.CreateSampleTourDomain();

        var tourPersistence = _mapper.Map<TourPersistence>(tourDomain);

        Assert.That(tourPersistence.TourLogPersistence, Has.Count.EqualTo(tourDomain.Logs.Count));
    }

    [Test]
    public void TourDomain_To_Tour_MapsTourLogsCorrectly()
    {
        var tourDomain = TestData.CreateSampleTourDomain();

        var tour = _mapper.Map<Tour>(tourDomain);

        Assert.That(tour.TourLogs, Has.Count.EqualTo(tourDomain.Logs.Count));
    }

    [Test]
    public void Tour_To_TourDomain_MapsTourLogsCorrectly()
    {
        var tour = TestData.CreateSampleTour();

        var tourDomain = _mapper.Map<TourDomain>(tour);

        Assert.That(tourDomain.Logs, Has.Count.EqualTo(tour.TourLogs.Count));
    }

    [Test]
    public void TourLogDomain_To_TourLogPersistence_MapsTourDomainAttributeCorrectly()
    {
        var tourLogDomain = TestData.CreateSampleTourLogDomain();

        var tourLogPersistence = _mapper.Map<TourLogPersistence>(tourLogDomain);

        Assert.Multiple(() => {

            Assert.That(tourLogPersistence.TourPersistenceId, Is.EqualTo(tourLogDomain.TourDomainId));
            Assert.That(tourLogPersistence.Id, Is.EqualTo(tourLogDomain.Id));
            Assert.That(tourLogPersistence.DateTime, Is.EqualTo(tourLogDomain.DateTime));
            Assert.That(tourLogPersistence.Comment, Is.EqualTo(tourLogDomain.Comment));
            Assert.That(tourLogPersistence.Difficulty, Is.EqualTo(tourLogDomain.Difficulty));
            Assert.That(tourLogPersistence.TotalDistance, Is.EqualTo(tourLogDomain.TotalDistance));
            Assert.That(tourLogPersistence.TotalTime, Is.EqualTo(tourLogDomain.TotalTime));
            Assert.That(tourLogPersistence.Rating, Is.EqualTo(tourLogDomain.Rating));
        });
    }

    [Test]
    public void TourLogPersistence_To_TourLogDomain_MapsAllPropertiesCorrectly()
    {
        var tourLogPersistence = TestData.CreateSampleTourLogPersistence();

        var tourLogDomain = _mapper.Map<TourLogDomain>(tourLogPersistence);

        Assert.Multiple(() => {
            Assert.That(tourLogDomain.TourDomainId, Is.EqualTo(tourLogPersistence.TourPersistenceId));
            Assert.That(tourLogDomain.Id, Is.EqualTo(tourLogPersistence.Id));
            Assert.That(tourLogDomain.DateTime, Is.EqualTo(tourLogPersistence.DateTime));
            Assert.That(tourLogDomain.Comment, Is.EqualTo(tourLogPersistence.Comment));
            Assert.That(tourLogDomain.Difficulty, Is.EqualTo(tourLogPersistence.Difficulty));
            Assert.That(tourLogDomain.TotalDistance, Is.EqualTo(tourLogPersistence.TotalDistance));
            Assert.That(tourLogDomain.TotalTime, Is.EqualTo(tourLogPersistence.TotalTime));
            Assert.That(tourLogDomain.Rating, Is.EqualTo(tourLogPersistence.Rating));
        });
    }

    [Test]
    public void TourLogDomain_To_TourLog_MapsAllPropertiesCorrectly()
    {
        var tourLogDomain = TestData.CreateSampleTourLogDomain();

        var tourLog = _mapper.Map<TourLog>(tourLogDomain);

        Assert.Multiple(() => {
            Assert.That(tourLog.TourId, Is.EqualTo(tourLogDomain.TourDomainId));
            Assert.That(tourLog.Id, Is.EqualTo(tourLogDomain.Id));
            Assert.That(tourLog.DateTime, Is.EqualTo(tourLogDomain.DateTime));
            Assert.That(tourLog.Comment, Is.EqualTo(tourLogDomain.Comment));
            Assert.That(tourLog.Difficulty, Is.EqualTo(tourLogDomain.Difficulty));
            Assert.That(tourLog.TotalDistance, Is.EqualTo(tourLogDomain.TotalDistance));
            Assert.That(tourLog.TotalTime, Is.EqualTo(tourLogDomain.TotalTime));
            Assert.That(tourLog.Rating, Is.EqualTo(tourLogDomain.Rating));
        });
    }

    [Test]
    public void TourLog_To_TourLogDomain_MapsAllPropertiesCorrectly()
    {
        var tourLog = TestData.CreateSampleTourLogDto();

        var tourLogDomain = _mapper.Map<TourLogDomain>(tourLog);

        Assert.Multiple(() => {
            Assert.That(tourLogDomain.TourDomainId, Is.EqualTo(tourLog.TourId));
            Assert.That(tourLogDomain.Id, Is.EqualTo(tourLog.Id));
            Assert.That(tourLogDomain.DateTime, Is.EqualTo(tourLog.DateTime));
            Assert.That(tourLogDomain.Comment, Is.EqualTo(tourLog.Comment));
            Assert.That(tourLogDomain.Difficulty, Is.EqualTo(tourLog.Difficulty));
            Assert.That(tourLogDomain.TotalDistance, Is.EqualTo(tourLog.TotalDistance));
            Assert.That(tourLogDomain.TotalTime, Is.EqualTo(tourLog.TotalTime));
            Assert.That(tourLogDomain.Rating, Is.EqualTo(tourLog.Rating));
        });
    }
}
