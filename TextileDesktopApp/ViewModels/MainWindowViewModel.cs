using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using TextileDesktopApp.Services;

namespace TextileDesktopApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _services;

        [ObservableProperty] private ViewModelBase? _currentRootViewModel;

        public MainWindowViewModel(IServiceProvider services, LoginPageViewModel loginPageViewModel)
        {
            _services = services;

            loginPageViewModel.LoginSucceeded += OnLoginSucceeded;
            CurrentRootViewModel = loginPageViewModel;
        }

        private void OnLoginSucceeded()
        {
            CurrentRootViewModel = _services.GetRequiredService<ShellViewModel>();
        }

        public MainWindowViewModel() { }
    }
}
