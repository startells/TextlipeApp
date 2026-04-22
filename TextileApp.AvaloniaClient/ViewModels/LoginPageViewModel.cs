using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TextileApp.AvaloniaClient.API.Services;
using TextileApp.AvaloniaClient.Services;

namespace TextileApp.AvaloniaClient.ViewModels;

public partial class LoginPageViewModel : ViewModelBase
{
    private readonly AuthService _authService;
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string? _username;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string? _password;

    [ObservableProperty]
    private string? _errorMessage;

    public event Action? LoginSucceeded;

    private bool CanLogin() =>
        !string.IsNullOrWhiteSpace(Username) &&
        !string.IsNullOrWhiteSpace(Password);
    
    public LoginPageViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        ErrorMessage = null;

        try
        {
            var response = await _authService.LoginAsync(Username, Password);
            
            if (response)
                LoginSucceeded?.Invoke();
        }
        catch (Exception e)
        {
            ErrorMessage = $"{e.Message}";
        }
    }
}