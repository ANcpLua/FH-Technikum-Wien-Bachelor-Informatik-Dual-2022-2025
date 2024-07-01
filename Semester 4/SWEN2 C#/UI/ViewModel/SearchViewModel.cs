using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using UI.Decorator;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel.Base;
using ILogger=Serilog.ILogger;

namespace UI.ViewModel;

public class SearchViewModel : BaseViewModel
{
    private readonly NavigationManager _navigationManager;
    private ObservableCollection<Tour> _searchResults = [];
    private string _searchText = string.Empty;

    public SearchViewModel(
        IHttpService httpService,
        IToastServiceWrapper toastServiceWrapper,
        ILogger logger,
        NavigationManager navigationManager
    )
        : base(httpService, toastServiceWrapper, logger)
    {
        _navigationManager = navigationManager;
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public ObservableCollection<Tour> SearchResults
    {
        get => _searchResults;
        set => SetProperty(ref _searchResults, value);
    }

    [UiMethodDecorator]
    public Task SearchToursAsync() => HandleApiRequestAsync(
    async () => {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            SearchResults.Clear();
            return;
        }

        var tours = await HttpService.GetListAsync<Tour>(
        $"api/tour/search/{SearchText.Trim()}"
        );
        SearchResults = new ObservableCollection<Tour>(
        tours?.Select(tour => {
            _ = tour.Popularity;
            _ = tour.AverageRating;
            _ = tour.IsChildFriendly;
            return tour;
        }) ??
        Array.Empty<Tour>()
        );

        if (!SearchResults.Any())
        {
            ToastServiceWrapper.ShowSuccess(
            "No tours found matching your search criteria."
            );
        }
    },
    "Error searching tours"
    );

    public void ClearSearch()
    {
        SearchText = string.Empty;
        SearchResults.Clear();
    }

    public void NavigateToTour(Guid tourId) => _navigationManager.NavigateTo($"/?tourId={tourId}");

    public async Task HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SearchToursAsync();
        }
    }
}
