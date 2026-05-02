using AutoService_Master.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AutoService_Master.Views;

public partial class ClientsView : UserControl
{
    public ClientsView()
    {
        InitializeComponent();
        
        DataContext = new ClientsViewModel();
    }
}