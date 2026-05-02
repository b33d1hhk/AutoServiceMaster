using System;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoService_Master.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isDarkMode = true;
    
    partial void OnIsDarkModeChanged(bool value)
    {
        Application.Current!.RequestedThemeVariant = value ? ThemeVariant.Dark : ThemeVariant.Light;
    }
    
    [ObservableProperty]
    private string _selectedLanguage = "Українська";
    
    [RelayCommand]
    private void ChangeLanguage(string langCode)
    {
        var app = Application.Current;
        if (app != null)
        {
            string dictPath = $"avares://AutoService Master/Resources/Lang.{langCode}.axaml";
            
            var newLanguageDict = new Avalonia.Markup.Xaml.Styling.ResourceInclude(new Uri(dictPath))
            {
                Source = new Uri(dictPath)
            };
            
            app.Resources.MergedDictionaries[0] = newLanguageDict;
        }
        
        if (langCode == "uk") SelectedLanguage = "Українська";
        else if (langCode == "en") SelectedLanguage = "English";
    }
}