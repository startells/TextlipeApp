using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TextileDesktopApp.ViewModels;

public partial class LoginPageViewModel : ViewModelBase
{
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

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private void Login()
    {
        // TODO: Здесь проверка пользователя через API/БД
        var ok = true;

        if (!ok)
        {
            ErrorMessage = "Неверный логин или пароль";
            return;
        }

        ErrorMessage = null;
        LoginSucceeded?.Invoke();
    }
}