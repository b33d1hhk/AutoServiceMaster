using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoService_Master.Models;
using AutoService_Master.Services;

namespace AutoService_Master.ViewModels;

public partial class OrdersViewModel : ViewModelBase
{
    private readonly JsonDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<Order> _orders;

    [ObservableProperty]
    private ObservableCollection<Client> _clients;

    [ObservableProperty]
    private Client? _selectedClient;

    [ObservableProperty]
    private string _newDescription = string.Empty;

    [ObservableProperty]
    private string _newPrice = string.Empty;

    public bool IsAdmin => AuthService.Instance.IsAdmin;

    public OrdersViewModel()
    {
        _dataService = new JsonDataService();
        _orders = new ObservableCollection<Order>(_dataService.LoadOrders());
        _clients = new ObservableCollection<Client>(_dataService.LoadClients());
    }

    [RelayCommand]
    private void SaveOrder()
    {
        if (SelectedClient == null || string.IsNullOrWhiteSpace(NewDescription))
        {
            return;
        }

        if (!decimal.TryParse(NewPrice, out decimal parsedPrice)) parsedPrice = 0;

        var newOrder = new Order
        {
            ClientId = SelectedClient.Id,
            ClientName = SelectedClient.FullName,
            Description = NewDescription,
            Price = parsedPrice,
            Status = OrderStatus.Pending
        };

        Orders.Add(newOrder);
        _dataService.SaveOrders(Orders);

        SelectedClient = null;
        NewDescription = string.Empty;
        NewPrice = string.Empty;
    }

    [RelayCommand]
    private void DeleteOrder(Order orderToDelete)
    {
        if (orderToDelete != null)
        {
            Orders.Remove(orderToDelete);
            _dataService.SaveOrders(Orders);
        }
    }
}