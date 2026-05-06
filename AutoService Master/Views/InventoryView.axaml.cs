using Avalonia.Controls;
using AutoService_Master.ViewModels;

namespace AutoService_Master.Views;

public partial class InventoryView : UserControl
{
    public InventoryView()
    {
        InitializeComponent();
        DataContext = new InventoryViewModel();
    }
}