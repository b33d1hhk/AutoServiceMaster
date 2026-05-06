using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoService_Master.Models;
using AutoService_Master.Services;

namespace AutoService_Master.ViewModels;

public partial class InventoryViewModel : ViewModelBase
{
    private readonly JsonDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<Part> _parts;

    [ObservableProperty]
    private string _newName = string.Empty;

    [ObservableProperty]
    private string _newArticle = string.Empty;

    [ObservableProperty]
    private string _newPrice = string.Empty;

    [ObservableProperty]
    private string _newQuantity = string.Empty;

    public bool IsAdmin => AuthService.Instance.IsAdmin;

    public InventoryViewModel()
    {
        _dataService = new JsonDataService();
        _parts = new ObservableCollection<Part>(_dataService.LoadParts());
    }

    [RelayCommand]
    private void SavePart()
    {
        if (string.IsNullOrWhiteSpace(NewName) || string.IsNullOrWhiteSpace(NewArticle))
        {
            return;
        }

        if (!decimal.TryParse(NewPrice, out decimal parsedPrice)) parsedPrice = 0;
        if (!int.TryParse(NewQuantity, out int parsedQuantity)) parsedQuantity = 0;

        var newPart = new Part
        {
            Name = NewName,
            Article = NewArticle,
            Price = parsedPrice,
            Quantity = parsedQuantity
        };

        Parts.Add(newPart);
        _dataService.SaveParts(Parts);

        NewName = string.Empty;
        NewArticle = string.Empty;
        NewPrice = string.Empty;
        NewQuantity = string.Empty;
    }

    [RelayCommand]
    private void DeletePart(Part partToDelete)
    {
        if (partToDelete != null)
        {
            Parts.Remove(partToDelete);
            _dataService.SaveParts(Parts);
        }
    }
}