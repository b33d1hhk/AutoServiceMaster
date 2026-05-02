using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoService_Master.Models;
using AutoService_Master.Services;

namespace AutoService_Master.ViewModels;

public partial class ClientsViewModel : ViewModelBase
{
    private readonly JsonDataService _dataService;
    
    [ObservableProperty]
    private ObservableCollection<Client> _clients;

    public ClientsViewModel()
    {
        _dataService = new JsonDataService();
        
        _clients = new ObservableCollection<Client>(_dataService.LoadClients());
    }
    
    [RelayCommand]
    private void AddClient()
    {
        var newClient = new Client 
        { 
            FullName = "Новий Клієнт", 
            PhoneNumber = "+380", 
            CarInfo = "Не вказано" 
        };
        
        Clients.Add(newClient);
        _dataService.SaveClients(Clients);
    }
}