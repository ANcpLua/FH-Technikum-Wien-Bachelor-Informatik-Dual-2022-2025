using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using Serilog;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel;

namespace Test.UI;

[TestFixture]
public class SearchViewModelTests
{
    private Mock<IHttpService> _mockHttpService;
    private Mock<IToastServiceWrapper> _mockToastService;
    private Mock<ILogger> _mockLogger;
    private Mock<NavigationManager> _mockNavigationManager;
    private SearchViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockHttpService = TestData.CreateMockHttpService();
        _mockToastService = TestData.CreateMockToastService();
        _mockLogger = TestData.CreateMockLogger();
        _mockNavigationManager = new Mock<NavigationManager>();

        _viewModel = new SearchViewModel(
            _mockHttpService.Object,
            _mockToastService.Object,
            _mockLogger.Object,
            _mockNavigationManager.Object
        );
    }

    [Test]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_viewModel.SearchText, Is.Empty);
            Assert.That(_viewModel.SearchResults, Is.Not.Null);
        });
        Assert.That(_viewModel.SearchResults, Is.Empty);
    }

    [Test]
    public void SearchText_SetProperty_RaisesPropertyChangedEvent()
    {
        var eventRaised = false;

        _viewModel.PropertyChanged += (_, _) => eventRaised = true;
        _viewModel.SearchText = TestData.ValidSearchText;

        Assert.That(eventRaised, Is.True);
    }

    [Test]
    public async Task SearchToursAsync_WithValidSearchText_PopulatesSearchResults()
    {
        const string searchText = TestData.ValidSearchText;
        var tours = TestData.CreateSampleTourList();
        _mockHttpService
            .Setup(s => s.GetListAsync<Tour>($"api/tour/search/{searchText}"))
            .ReturnsAsync(tours);

        _viewModel.SearchText = searchText;
        await _viewModel.SearchToursAsync();

        Assert.That(_viewModel.SearchResults, Is.EqualTo(tours));
    }

    [Test]
    public async Task SearchToursAsync_WithEmptySearchText_ClearsSearchResults()
    {
        _viewModel.SearchResults = new ObservableCollection<Tour>(TestData.CreateSampleTourList());
        _viewModel.SearchText = string.Empty;

        await _viewModel.SearchToursAsync();

        Assert.That(_viewModel.SearchResults, Is.Empty);
        _mockHttpService.Verify(s => s.GetListAsync<Tour>(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task SearchToursAsync_WithNoResults_ShowsToastMessage()
    {
        _mockHttpService
            .Setup(s => s.GetListAsync<Tour>(It.IsAny<string>()))
            .ReturnsAsync((IEnumerable<Tour>)null!);

        _viewModel.SearchText = TestData.InvalidSearchText;
        await _viewModel.SearchToursAsync();

        Assert.That(_viewModel.SearchResults, Is.Empty);
        _mockToastService.Verify(
            t => t.ShowSuccess("No tours found matching your search criteria."),
            Times.Once
        );
    }

    [Test]
    public void ClearSearch_ResetsSearchTextAndResults()
    {
        _viewModel.SearchText = TestData.ValidSearchText;
        _viewModel.SearchResults = new ObservableCollection<Tour>(TestData.CreateSampleTourList());

        _viewModel.ClearSearch();

        Assert.Multiple(() =>
        {
            Assert.That(_viewModel.SearchText, Is.Empty);
            Assert.That(_viewModel.SearchResults, Is.Empty);
        });
    }

    [Test]
    public async Task HandleKeyPress_EnterKey_CallsSearchToursAsyncWithCorrectSearchText()
    {
        var keyboardEventArgs = new KeyboardEventArgs
        {
            Key = "Enter"
        };
        _viewModel.SearchText = TestData.ValidSearchText;

        await _viewModel.HandleKeyPress(keyboardEventArgs);

        _mockHttpService.Verify(
            s => s.GetListAsync<Tour>($"api/tour/search/{TestData.ValidSearchText}"),
            Times.Once
        );
    }

    [Test]
    public async Task HandleKeyPress_NonEnterKey_DoesNotCallSearchToursAsync()
    {
        var keyboardEventArgs = new KeyboardEventArgs
        {
            Key = "A"
        };

        await _viewModel.HandleKeyPress(keyboardEventArgs);

        _mockHttpService.Verify(s => s.GetListAsync<Tour>(It.IsAny<string>()), Times.Never);
    }
}