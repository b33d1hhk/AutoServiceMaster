using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AutoService_Master.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        MainContentArea.Content = new DashboardView();
    }
    private void NavDashboard_Click(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = new DashboardView();
    }

    private void NavClients_Click(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = new ClientsView();
    }
}