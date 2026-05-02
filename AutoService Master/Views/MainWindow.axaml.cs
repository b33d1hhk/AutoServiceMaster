using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AutoService_Master.Views;

public partial class MainWindow : Window
{
    private readonly DashboardView _dashboardView;
    private readonly ClientsView _clientsView;
    private readonly SettingsView _settingsView;
    public MainWindow()
    {
        InitializeComponent();
        
        _dashboardView = new DashboardView();
        _clientsView = new ClientsView();
        _settingsView = new SettingsView();

        MainContentArea.Content = _clientsView;
    }
    private void NavDashboard_Click(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = _dashboardView;
    }

    private void NavClients_Click(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = _clientsView;
    }
    private void NavSettings_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainContentArea.Content = _settingsView;
    }
}