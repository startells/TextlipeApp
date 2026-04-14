using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TextileApp.AvaloniaClient.Services;

namespace TextileApp.AvaloniaClient.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    private readonly MaterialsPageViewModel _materialsPageViewModel;
    private readonly ClientsPageViewModel _clientsPageViewModel;

    public NavigationService ContentNavigation { get; }

    public ShellViewModel(
        NavigationService contentNavigation,
        MaterialsPageViewModel materialsPageViewModel,
        ClientsPageViewModel clientsPageViewModel)
    {
        ContentNavigation = contentNavigation;
        _materialsPageViewModel = materialsPageViewModel;
        _clientsPageViewModel = clientsPageViewModel;

        ContentNavigation.PropertyChanged += OnNavigationPropertyChanged;

        // Первая страница после логина
        ContentNavigation.Reset(_materialsPageViewModel);
    }

    private void OnNavigationPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NavigationService.CanGoBack))
            BackCommand.NotifyCanExecuteChanged();

        if (e.PropertyName == nameof(NavigationService.CanGoForward))
            ForwardCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void GoMaterials() => ContentNavigation.NavigateTo(_materialsPageViewModel, false);

    [RelayCommand]
    private void GoClients() => ContentNavigation.NavigateTo(_clientsPageViewModel);

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void Back() => ContentNavigation.GoBack();

    [RelayCommand(CanExecute = nameof(CanGoForward))]
    private void Forward() => ContentNavigation.GoForward();

    private bool CanGoBack() => ContentNavigation.CanGoBack;
    private bool CanGoForward() => ContentNavigation.CanGoForward;
}