using System;
using AutoService_Master.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoService_Master.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;
    
    public event Action? OnLoginSuccess;

    [RelayCommand]
    private void Login()
    {
        if (AuthService.Instance.Login(Username, Password))
        {
            ErrorMessage = string.Empty;
            OnLoginSuccess?.Invoke();
        }
        else
        {
            ErrorMessage = "Невірний логін або пароль";
        }
    }
}