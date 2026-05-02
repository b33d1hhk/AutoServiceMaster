using AutoService_Master.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoService_Master.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly JsonDataService _dataService;
    
    [ObservableProperty]
    private int _totalClients;
    [ObservableProperty]
    private int _activeOrders;
    [ObservableProperty]
    private string _todayRevenue = string.Empty;

    public DashboardViewModel()
    {
        _dataService = new JsonDataService();
        UpdateStatistics();
    }
    
    public void UpdateStatistics()
    {
        var clients = _dataService.LoadClients();
        TotalClients = clients.Count;

        ActiveOrders = 14; 
        TodayRevenue = "₴ 12 500";
    }
}