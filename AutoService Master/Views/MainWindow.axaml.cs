using AutoService_Master.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AutoService_Master.Views;

public partial class MainWindow : Window
{
    private DashboardView? _dashboardView;
    private ClientsView? _clientsView;
    private SettingsView? _settingsView;
    private OrdersView? _ordersView;
    private InventoryView? _inventoryView;

    public MainWindow()
    {
        InitializeComponent();
        
        var loginView = new LoginView();
        var loginViewModel = new LoginViewModel();
        
        loginViewModel.OnLoginSuccess += () => 
        {
            LoginContentArea.IsVisible = false;
            MainAppLayout.IsVisible = true;
            
            _dashboardView = new DashboardView 
            { 
                DataContext = new DashboardViewModel()
            };
            _clientsView = new ClientsView();
            _settingsView = new SettingsView();
            _ordersView = new OrdersView();
            _inventoryView = new InventoryView();
            
            MainContentArea.Content = _dashboardView;
        };

        loginView.DataContext = loginViewModel;
        LoginContentArea.Content = loginView;
    }
    private void NavDashboard_Click(object sender, RoutedEventArgs e)
    {
        if (_dashboardView?.DataContext is DashboardViewModel viewModel)
        {
            viewModel.UpdateStatistics();
        }
        MainContentArea.Content = _dashboardView;
    }

    private void NavClients_Click(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = _clientsView;
    }

    private void NavOrders_Click(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = _ordersView;
    }

    private void NavInventory_Click(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = _inventoryView;
    }

    private void NavSettings_Click(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = _settingsView;
    }
}