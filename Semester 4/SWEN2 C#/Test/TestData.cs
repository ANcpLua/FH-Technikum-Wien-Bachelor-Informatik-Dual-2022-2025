using System.Text;
using System.Text.Json;
using BL.DomainModel;
using BlazorDownloadFile;
using DAL.PersistenceModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Moq;
using Serilog;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel;

namespace Test;

public static class TestData
{
    public const string ValidSearchText = "Sample Tour";
    public const string InvalidSearchText = "NonexistentTour";
    private const double TestTimeSpan = 60.0;
    public static readonly Guid TestGuid = new("11111111-1111-1111-1111-111111111111");
    private static readonly DateTime TestDateTime = new(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    public static readonly Guid NonexistentGuid = new("99999999-9999-9999-9999-999999999999");

    public static TourPersistence CreateSampleTourPersistence() => new()
    {
        Id = TestGuid,
        Name = "Sample Tour",
        Description = "This is a sample tour for testing",
        From = "Start City",
        To = "End City",
        Distance = 100.5,
        EstimatedTime = TestTimeSpan,
        TransportType = "Car",
        ImagePath = "/images/sample.png",
        RouteInformation = "Sample route information",
        TourLogPersistence = []
    };

    public static Tour CreateSampleTour() => new()
    {
        Id = TestGuid,
        Name = "Sample Tour",
        Description = "Sample tour for testing",
        From = "City1",
        To = "City2",
        Distance = 100.5,
        EstimatedTime = TestTimeSpan,
        TransportType = "Car",
        ImagePath = "/images/sample.png",
        RouteInformation = "Sample route information",
        TourLogs = []
    };

    public static List<Tour> CreateSampleTourList(int count = 5)
    {
        List<Tour> tours = [];
        for (var i = 0; i < count; i++)
        {
            tours.Add(CreateSampleTour());
        }

        return tours;
    }

    public static TourLogPersistence CreateSampleTourLogPersistence() => new()
    {
        Id = new Guid("00000000-0000-0000-0000-000000000001"),
        TourPersistenceId = TestGuid,
        DateTime = TestDateTime,
        Comment = "Sample tour log comment",
        Difficulty = 3,
        TotalDistance = 50.25,
        TotalTime = 60,
        Rating = 4
    };

    public static List<TourPersistence> CreateSampleTourPersistenceList() =>
    [
        CreateSampleTourPersistence(),
        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000002"),
            Name = "Another Tour",
            Description = "This is another sample tour",
            From = "Another Start",
            To = "Another End",
            Distance = 75.3,
            EstimatedTime = TestTimeSpan,
            TransportType = "Bicycle",
            ImagePath = "/images/another.png",
            RouteInformation = "Another route information",
            TourLogPersistence = []
        }
    ];

    public static List<TourLogPersistence> CreateSampleTourLogPersistenceList() =>
    [
        CreateSampleTourLogPersistence(),
        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
            TourPersistenceId = TestGuid,
            DateTime = TestDateTime.AddDays(1),
            Comment = "Another sample tour log comment",
            Difficulty = 2,
            TotalDistance = 30.5,
            TotalTime = 60,
            Rating = 5
        }
    ];

    public static TourDomain CreateSampleTourDomain() => new()
    {
        Id = TestGuid,
        Name = "Sample Tour Domain",
        Description = "This is a sample tour domain for testing",
        From = "Start City Domain",
        To = "End City Domain",
        Distance = 100.5,
        EstimatedTime = TestTimeSpan,
        TransportType = "Car Domain",
        ImagePath = "/images/sample_domain.png",
        RouteInformation = "Sample route information domain",
        Logs = []
    };

    public static TourLogDomain CreateSampleTourLogDomain() => new()
    {
        Id = new Guid("00000000-0000-0000-0000-000000000001"),
        TourDomainId = TestGuid,
        DateTime = TestDateTime,
        Comment = "Sample tour log domain comment",
        Difficulty = 3,
        TotalDistance = 50.25,
        TotalTime = 60,
        Rating = 4
    };

    public static List<TourDomain> CreateSampleTourDomainList() =>
    [
        CreateSampleTourDomain(),
        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000002"),
            Name = "Another Tour Domain",
            Description = "This is another sample tour domain",
            From = "Another Start Domain",
            To = "Another End Domain",
            Distance = 75.3,
            EstimatedTime = TimeSpan.FromHours(1.5).TotalMinutes,
            TransportType = "Bicycle Domain",
            ImagePath = "/images/another_domain.png",
            RouteInformation = "Another route information domain",
            Logs = []
        }
    ];

    public static List<TourLogDomain> CreateSampleTourLogDomainList() =>
    [
        CreateSampleTourLogDomain(),
        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
            TourDomainId = TestGuid,
            DateTime = TestDateTime.AddDays(1),
            Comment = "Another sample tour log domain comment",
            Difficulty = 2,
            TotalDistance = 30.5,
            TotalTime = 45,
            Rating = 5
        }
    ];

    private static Tour CreateSampleTourDto() => new()
    {
        Id = TestGuid,
        Name = "Sample Tour DTO",
        Description = "This is a sample tour DTO for testing",
        From = "Start City DTO",
        To = "End City DTO",
        Distance = 100.5,
        EstimatedTime = TestTimeSpan,
        TransportType = "Car DTO",
        ImagePath = "/images/sample_dto.png",
        RouteInformation = "Sample route information DTO",
        TourLogs = []
    };

    public static TourLog CreateSampleTourLogDto() => new()
    {
        Id = new Guid("00000000-0000-0000-0000-000000000001"),
        TourId = TestGuid,
        DateTime = TestDateTime,
        Comment = "Sample tour log DTO comment",
        Difficulty = 3,
        TotalDistance = 50.25,
        TotalTime = 60,
        Rating = 4
    };

    public static List<TourLog> CreateSampleTourLogDtoList() =>
    [
        CreateSampleTourLogDto(),
        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
            TourId = TestGuid,
            DateTime = TestDateTime.AddDays(1),
            Comment = "Another sample tour log DTO comment",
            Difficulty = 2,
            TotalDistance = 30.5,
            TotalTime = 45,
            Rating = 5
        }
    ];

    public static string CreateSampleTourDomainJson()
    {
        var tour = CreateSampleTourDomain();
        return JsonSerializer.Serialize(tour);
    }

    public static string CreateSampleTourJson()
    {
        var tour = CreateSampleTourDto();
        return JsonSerializer.Serialize(tour);
    }

    public static Mock<IToastServiceWrapper> CreateMockToastService()
    {
        Mock<IToastServiceWrapper> mockToastService = new();

        mockToastService.Setup(t => t.ShowSuccess(It.IsAny<string>()));
        mockToastService.Setup(t => t.ShowError(It.IsAny<string>()));

        return mockToastService;
    }

    public static Mock<IViewModelHelperService> CreateMockViewModelHelperService()
    {
        Mock<IViewModelHelperService> mockViewModelHelperService = new();

        mockViewModelHelperService
            .Setup(v => v.ToggleVisibility(ref It.Ref<bool>.IsAny))
            .Returns(true);
        mockViewModelHelperService
            .Setup(v => v.ToggleVisibility(ref It.Ref<bool>.IsAny))
            .Returns(false);
        mockViewModelHelperService.Setup(v => v.ShowForm(ref It.Ref<bool>.IsAny));
        mockViewModelHelperService.Setup(v =>
            v.ResetForm(ref It.Ref<Tour>.IsAny, It.IsAny<Func<Tour>>())
        );

        return mockViewModelHelperService;
    }

    public static Mock<MapViewModel> CreateMockMapViewModel() => new(
        Mock.Of<IJSRuntime>(),
        Mock.Of<IHttpService>(),
        Mock.Of<IToastServiceWrapper>(),
        Mock.Of<ILogger>()
    )
    {
        CallBase = true
    };

    public static Tour CreateSampleTourWithVariousProperties() => new()
    {
        Id = TestGuid,
        Name = "Sample Tour",
        Description = "Sample tour for testing",
        From = "City1",
        To = "City2",
        Distance = Random.Shared.Next(50, 200),
        EstimatedTime = Random.Shared.Next(60, 480),
        TransportType = "Car",
        ImagePath = "/images/sample.png",
        RouteInformation = "Sample route information",
        TourLogs = []
    };

    public static Mock<IConfiguration> CreateMockConfiguration()
    {
        Mock<IConfiguration> mockConfiguration = new();
        mockConfiguration
            .Setup(c => c[It.Is<string>(s => s == "AppSettings:OpenRouteServiceApiKey")])
            .Returns("dummy-api-key");
        mockConfiguration
            .Setup(c => c[It.Is<string>(s => s == "AppSettings:OpenRouteServiceApiBaseUrl")])
            .Returns("https://api.openrouteservice.org");
        mockConfiguration
            .Setup(c => c[It.Is<string>(s => s == "AppSettings:ImageBasePath")])
            .Returns("/images/");
        return mockConfiguration;
    }

    public static Mock<IHttpService> CreateMockHttpService()
    {
        Mock<IHttpService> mockHttpService = new();

        mockHttpService
            .Setup(s => s.GetAsync<Tour>(It.IsAny<string>()))
            .ReturnsAsync(CreateSampleTour());
        mockHttpService
            .Setup(s => s.GetListAsync<Tour>(It.IsAny<string>()))
            .ReturnsAsync(CreateSampleTourList());
        mockHttpService
            .Setup(s => s.PostAsync<Tour>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(CreateSampleTour());
        mockHttpService
            .Setup(s => s.PutAsync<Tour>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(CreateSampleTour());
        mockHttpService.Setup(s => s.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
        mockHttpService
            .Setup(s => s.GetStringAsync(It.IsAny<string>()))
            .ReturnsAsync("Sample string response");
        mockHttpService
            .Setup(s => s.GetByteArrayAsync(It.IsAny<string>()))
            .ReturnsAsync([
                1, 2, 3, 4, 5
            ]);

        return mockHttpService;
    }

    public static Mock<IRouteApiService> CreateMockRouteApiService()
    {
        Mock<IRouteApiService> mockRouteApiService = new();

        mockRouteApiService
            .Setup(r =>
                r.FetchRouteDataAsync(
                    It.IsAny<(double, double)>(),
                    It.IsAny<(double, double)>(),
                    It.IsAny<string>()
                )
            )
            .ReturnsAsync((100.5, 60.0));

        return mockRouteApiService;
    }

    public static Mock<IJSRuntime> CreateMockJsRuntime()
    {
        Mock<IJSRuntime> mockJsRuntime = new();
        return mockJsRuntime;
    }

    public static Mock<IBlazorDownloadFileService> CreateMockBlazorDownloadFileService()
    {
        Mock<IBlazorDownloadFileService> mockBlazorDownloadFile = new();
        mockBlazorDownloadFile.Setup(b =>
            b.DownloadFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>())
        );
        return mockBlazorDownloadFile;
    }

    public static Mock<IBrowserFile> CreateMockBrowserFile(string content)
    {
        Mock<IBrowserFile> mockFile = new();
        MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(content));
        mockFile
            .Setup(f => f.OpenReadStream(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns(memoryStream);
        return mockFile;
    }

    public static Mock<ILogger> CreateMockLogger() => new();

    public static (double Latitude, double Longitude) CreateSampleCoordinates() => (48.2082, 16.3738);

    public static void SetupMapViewModel(
        Mock<MapViewModel> mockMapViewModel,
        (double Latitude, double Longitude) fromCoords,
        (double Latitude, double Longitude) toCoords
    ) => mockMapViewModel
        .Setup(m => m.GetCoordinates(It.IsAny<string>()))
        .Returns((string city) => city == "From" ? fromCoords : toCoords);

    public static void SetupRouteApiService(
        Mock<IRouteApiService> mockRouteApiService,
        (double Latitude, double Longitude) fromCoords,
        (double Latitude, double Longitude) toCoords
    ) => mockRouteApiService
        .Setup(r => r.FetchRouteDataAsync(fromCoords, toCoords, It.IsAny<string>()))
        .ReturnsAsync((100.5, 60.0));

    public static void SetupHttpServicePut(Mock<IHttpService> mockHttpService, Tour tour) => mockHttpService
        .Setup(s => s.PutAsync<Tour>($"api/tour/{tour.Id}", tour))
        .ReturnsAsync(tour);
}