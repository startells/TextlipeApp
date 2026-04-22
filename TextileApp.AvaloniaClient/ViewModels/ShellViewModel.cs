using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using TextileApp.AvaloniaClient.Models;
using TextileApp.AvaloniaClient.Services;
using TextileApp.Domain.Constants;

namespace TextileApp.AvaloniaClient.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    private readonly MaterialsPageViewModel _materialsPageViewModel;
    private readonly ClientsPageViewModel _clientsPageViewModel;
    private readonly SessionService _sessionService;
    
    public NavigationService ContentNavigation { get; }
    public List<NavItem> NavigationItems { get; private set; }
    public IEnumerable<NavItem> VisibleItems =>
        NavigationItems.Where(i => i.IsVisible());
    
    public ShellViewModel(
        NavigationService contentNavigation,
        MaterialsPageViewModel materialsPageViewModel,
        ClientsPageViewModel clientsPageViewModel,
        SessionService sessionService)
    {
        ContentNavigation = contentNavigation;
        _materialsPageViewModel = materialsPageViewModel;
        _clientsPageViewModel = clientsPageViewModel;
        _sessionService = sessionService;
        
        BuildNavigationMenu();
        
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

    private void BuildNavigationMenu()
    {
        NavigationItems = new()
        {
                new NavItem
                {
                    Title = "Материалы",
                    Command = new RelayCommand(GoMaterials),
                    IsVisible = () => _sessionService.HasRole(RolesConstants.Manager)
                },
                new NavItem
                {
                    Title = "Клиенты",
                    Command = new RelayCommand(GoClients),
                    IsVisible = () => _sessionService.HasRole(RolesConstants.Admin)
                }
        };
    }
}