using Microsoft.JSInterop;
using Moq;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel;
using ILogger=Serilog.ILogger;

namespace Test;

[TestFixture]
public class TourLogViewModelTests
{
    [SetUp]
    public void Setup()
    {
        _mockHttpService = TestData.CreateMockHttpService();
        _mockToastService = TestData.CreateMockToastService();
        _mockJsRuntime = TestData.CreateMockJsRuntime();
        _mockLogger = TestData.CreateMockLogger();
        _mockViewModelHelperService = TestData.CreateMockViewModelHelperService();

        _viewModel = new TourLogViewModel(
        _mockHttpService.Object,
        _mockToastService.Object,
        _mockJsRuntime.Object,
        _mockLogger.Object,
        _mockViewModelHelperService.Object
        );
    }

    private Mock<IHttpService> _mockHttpService;
    private Mock<IToastServiceWrapper> _mockToastService;
    private Mock<IJSRuntime> _mockJsRuntime;
    private Mock<IViewModelHelperService> _mockViewModelHelperService;
    private Mock<ILogger> _mockLogger;
    private TourLogViewModel _viewModel;

    [Test]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        Assert.That(_viewModel.TourLogs, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(_viewModel.TourLogs, Is.Empty);
            Assert.That(_viewModel.SelectedTourLog, Is.Not.Null);
            Assert.That(_viewModel.IsLogFormVisible, Is.False);
            Assert.That(_viewModel.IsEditing, Is.False);
            Assert.That(_viewModel.SelectedTourId, Is.Null);
        });
    }

    [Test]
    public void SelectedTourLog_SetProperty_RaisesPropertyChangedEvent()
    {
        var newTourLog = TestData.CreateSampleTourLogDto();
        var eventRaised = false;

        _viewModel.PropertyChanged += (_, _) => eventRaised = true;
        _viewModel.SelectedTourLog = newTourLog;

        Assert.Multiple(() => {
            Assert.That(_viewModel.SelectedTourLog, Is.EqualTo(newTourLog));
            Assert.That(eventRaised, Is.True);
        });
    }

    [Test]
    public async Task SelectedTourId_WhenSet_CallsHandleTourSelection()
    {
        var newTourId = TestData.CreateSampleTour().Id;
        _mockHttpService
            .Setup(s => s.GetListAsync<TourLog>(It.IsAny<string>()))
            .ReturnsAsync(TestData.CreateSampleTourLogDtoList());

        await _viewModel.LoadTourLogsAsync();
        _viewModel.SelectedTourId = newTourId;

        Assert.That(_viewModel.SelectedTourId, Is.EqualTo(newTourId));
        _mockHttpService.Verify(s => s.GetListAsync<TourLog>(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void IsFormValid_ValidData_ReturnsTrue()
    {
        _viewModel.SelectedTourLog = new TourLog
        {
            Comment = "Valid comment",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = 60,
            Rating = 4
        };

        Assert.That(_viewModel.IsFormValid, Is.True);
    }

    [Test]
    public void IsFormValid_InvalidData_ReturnsFalse()
    {
        _viewModel.SelectedTourLog = new TourLog
        {
            Comment = "",
            Difficulty = 0,
            TotalDistance = 0,
            TotalTime = 0,
            Rating = 0
        };

        Assert.That(_viewModel.IsFormValid, Is.False);
    }

    [Test]
    public void ShowAddLogForm_WhenTourIdIsNull_DoesNotShowForm()
    {
        _viewModel.SelectedTourId = null;
        _viewModel.ToggleLogForm();

        Assert.Multiple(() => {
            Assert.That(_viewModel.IsLogFormVisible, Is.False);
            Assert.That(_viewModel.IsEditing, Is.False);
        });
        _mockViewModelHelperService.Verify(v => v.ShowForm(ref It.Ref<bool>.IsAny), Times.Never);
    }

    [Test]
    public async Task SaveTourLogAsync_ValidNewLog_SavesSuccessfully()
    {
        var newLog = TestData.CreateSampleTourLogDto();
        newLog.Id = Guid.Empty;
        _viewModel.SelectedTourId = TestData.CreateSampleTour().Id;
        _viewModel.SelectedTourLog = newLog;

        _mockHttpService
            .Setup(s => s.PostAsync<TourLog>(It.IsAny<string>(), It.IsAny<TourLog>()))
            .ReturnsAsync(newLog);

        var result = await _viewModel.SaveTourLogAsync();

        Assert.That(result, Is.True);
        _mockHttpService.Verify(
        s => s.PostAsync<TourLog>("api/tourlog", It.IsAny<TourLog>()),
        Times.Once
        );
        _mockToastService.Verify(t => t.ShowSuccess("Tour log created successfully."), Times.Once);
    }

    [Test]
    public async Task SaveTourLogAsync_ValidExistingLog_UpdatesSuccessfully()
    {
        var existingLog = TestData.CreateSampleTourLogDto();
        _viewModel.SelectedTourId = TestData.CreateSampleTour().Id;
        _viewModel.SelectedTourLog = existingLog;

        _mockHttpService
            .Setup(s => s.PutAsync<TourLog>(It.IsAny<string>(), It.IsAny<TourLog>()))
            .ReturnsAsync(existingLog);

        var result = await _viewModel.SaveTourLogAsync();

        Assert.That(result, Is.True);
        _mockHttpService.Verify(
        s => s.PutAsync<TourLog>($"api/tourlog/{existingLog.Id}", It.IsAny<TourLog>()),
        Times.Once
        );
        _mockToastService.Verify(t => t.ShowSuccess("Tour log updated successfully."), Times.Once);
    }

    [Test]
    public async Task SaveTourLogAsync_InvalidForm_ReturnsFalse()
    {
        _viewModel.SelectedTourId = TestData.CreateSampleTour().Id;
        _viewModel.SelectedTourLog = new TourLog();// Invalid log

        var result = await _viewModel.SaveTourLogAsync();

        Assert.That(result, Is.False);
        _mockToastService.Verify(t => t.ShowError("Not Valid."), Times.Once);
    }

    [Test]
    public async Task EditHandleTourLogAction_ExistingLog_LoadsLogForEditing()
    {
        var existingLog = TestData.CreateSampleTourLogDto();
        _mockHttpService
            .Setup(s => s.GetAsync<TourLog>(It.IsAny<string>()))
            .ReturnsAsync(existingLog);

        await _viewModel.EditHandleTourLogAction(existingLog.Id);

        Assert.Multiple(() => {
            Assert.That(_viewModel.SelectedTourLog, Is.EqualTo(existingLog));
            Assert.That(_viewModel.SelectedTourId, Is.EqualTo(existingLog.TourId));
            Assert.That(_viewModel.IsLogFormVisible, Is.True);
            Assert.That(_viewModel.IsEditing, Is.True);
        });
    }

    [Test]
    public async Task EditHandleTourLogAction_NewLog_DoesNotLoadLog()
    {
        await _viewModel.EditHandleTourLogAction(Guid.Empty);

        Assert.Multiple(() => {
            Assert.That(_viewModel.SelectedTourLog, Is.Not.Null);
            Assert.That(_viewModel.SelectedTourId, Is.Null);
            Assert.That(_viewModel.IsLogFormVisible, Is.False);
            Assert.That(_viewModel.IsEditing, Is.False);
        });
    }

    [Test]
    public async Task DeleteTourLogAsync_UserConfirms_DeletesLog()
    {
        var logId = TestData.TestGuid;
        _mockJsRuntime
            .Setup(j => j.InvokeAsync<bool>(It.IsAny<string>(), It.IsAny<object[]>()))
            .ReturnsAsync(true);

        await _viewModel.DeleteTourLogAsync(logId);

        _mockHttpService.Verify(s => s.DeleteAsync($"api/tourlog/{logId}"), Times.Once);
        _mockToastService.Verify(t => t.ShowSuccess("Tour log deleted successfully."), Times.Once);
    }

    [Test]
    public async Task DeleteTourLogAsync_UserCancels_DoesNotDeleteLog()
    {
        var logId = TestData.TestGuid;
        _mockJsRuntime
            .Setup(j => j.InvokeAsync<bool>(It.IsAny<string>(), It.IsAny<object[]>()))
            .ReturnsAsync(false);

        await _viewModel.DeleteTourLogAsync(logId);

        _mockHttpService.Verify(s => s.DeleteAsync(It.IsAny<string>()), Times.Never);
        _mockToastService.Verify(t => t.ShowSuccess(It.IsAny<string>()), Times.Never);
    }
}
