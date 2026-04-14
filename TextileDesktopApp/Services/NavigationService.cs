using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using TextileDesktopApp.ViewModels;

namespace TextileDesktopApp.Services;

public partial class NavigationService : ObservableObject
{
    private readonly Stack<ViewModelBase> _backStack = new Stack<ViewModelBase>();
    private readonly Stack<ViewModelBase> _forwardStack = new Stack<ViewModelBase>();
    
    [ObservableProperty]
    private ViewModelBase? _currentViewModel;
    
    public bool CanGoBack => _backStack.Count > 0;
    public bool CanGoForward => _forwardStack.Count > 0;
    
    public void NavigateTo(ViewModelBase viewModel, bool addToHistory = true)
    {
        if (CurrentViewModel is not null && addToHistory)
        {
            _backStack.Push(CurrentViewModel);
        }
        
        CurrentViewModel = viewModel;
        _forwardStack.Clear();
        
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
    }

    public void GoBack()
    {
        if (!CanGoBack)
            return;
        
        if (CurrentViewModel is not null)
        {
            _forwardStack.Push(CurrentViewModel);
        }
        
        CurrentViewModel = _backStack.Pop();
        
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
    }

    public void GoForward()
    {
        if (!CanGoForward)
            return;
        
        if (CurrentViewModel is not null)
        {
            _backStack.Push(CurrentViewModel);
        }
        
        CurrentViewModel = _forwardStack.Pop();
        
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
    }
    
    public void Reset(ViewModelBase viewModel)
    {
        _backStack.Clear();
        _forwardStack.Clear();
        CurrentViewModel =  viewModel;
        
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
    }
}