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
    [ObservableProperty]
    private string _newFullName = string.Empty;
    [ObservableProperty]
    private string _newPhone = string.Empty;
    [ObservableProperty]
    private string _newCarInfo = string.Empty;

    private Client? _editingClient;
    public bool IsAdmin => AuthService.Instance.IsAdmin;
    public ClientsViewModel()
    {
        _dataService = new JsonDataService();
        _clients = new ObservableCollection<Client>(_dataService.LoadClients());
    }
    [RelayCommand]
    private void EditClient(Client clientToEdit)
    {
        _editingClient = clientToEdit;
            
        NewFullName = clientToEdit.FullName;
        NewPhone = clientToEdit.PhoneNumber;
        NewCarInfo = clientToEdit.CarInfo;
    }
    
    [RelayCommand]
    private void SaveClient()
    {
        if (string.IsNullOrWhiteSpace(NewFullName))
        {
            return; 
        }

        if (_editingClient == null)
        {   
            var newClient = new Client 
            { 
                FullName = NewFullName, 
                PhoneNumber = NewPhone, 
                CarInfo = NewCarInfo 
            };
            Clients.Add(newClient);
        }
        else
        {
            _editingClient.FullName = NewFullName;
            _editingClient.PhoneNumber = NewPhone;
            _editingClient.CarInfo = NewCarInfo;
            
            int index = Clients.IndexOf(_editingClient);
            Clients[index] = _editingClient;

            _editingClient = null;
        }
        _dataService.SaveClients(Clients);
        
        NewFullName = string.Empty;
        NewPhone = string.Empty;
        NewCarInfo = string.Empty;
    }

    [RelayCommand]
    private void DeleteClient(Client clientToDelete)
    {
        if (clientToDelete != null)
        {
            Clients.Remove(clientToDelete);
            _dataService.SaveClients(Clients);
        }
    }
}