using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoService_Master.Models;
using AutoService_Master.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

    [ObservableProperty]
    private ObservableCollection<Order> _recentOrders = new();

    [ObservableProperty]
    private ObservableCollection<string> _purchasingRecommendations = new();

    private Stack<string> _reportHistory = new Stack<string>();

    public DashboardViewModel()
    {
        _dataService = new JsonDataService();
        UpdateStatistics();
    }

    public void UpdateStatistics()
    {
        var clients = _dataService.LoadClients();
        var orders = _dataService.LoadOrders();
        var parts = _dataService.LoadParts();

        TotalClients = clients.Count;
        ActiveOrders = orders.Count(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.InProgress);
        decimal revenue = orders
            .Where(o => o.CreatedAt.Date == DateTime.Today && (o.Status == OrderStatus.Completed || o.Status == OrderStatus.Paid))
            .Sum(o => o.Price);
        TodayRevenue = $"₴ {revenue}";

        var recent = orders.OrderByDescending(o => o.CreatedAt).Take(5);
        RecentOrders = new ObservableCollection<Order>(recent);

        GenerateSmartPurchasingReport(orders.ToList(), parts);
    }

    private void GenerateSmartPurchasingReport(List<Order> orders, List<Part> parts)
    {
        Dictionary<string, int> partUsage = new Dictionary<string, int>();

        HashSet<string> urgentRestock = new HashSet<string>();

        foreach (var order in orders)
        {
            if (string.IsNullOrWhiteSpace(order.Description)) continue;
            string descLower = order.Description.ToLower();

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part.Name)) continue;

                if (descLower.Contains(part.Name.ToLower()) || (!string.IsNullOrEmpty(part.Article) && descLower.Contains(part.Article.ToLower())))
                {
                    if (partUsage.ContainsKey(part.Article))
                        partUsage[part.Article]++;
                    else
                        partUsage[part.Article] = 1;
                }
            }
        }

        var recommendations = new List<string>();

        foreach (var part in parts)
        {
            int usedCount = partUsage.ContainsKey(part.Article) ? partUsage[part.Article] : 0;

            if (usedCount > 0 && part.Quantity <= usedCount * 2)
            {
                urgentRestock.Add(part.Article);
                int toBuy = (usedCount * 3) - part.Quantity;
                recommendations.Add($"⚠️ {part.Name} (Арт. {part.Article}): Використано {usedCount} раз(ів). Залишок: {part.Quantity}. Рекомендовано замовити: {toBuy} шт.");
            }
            else if (part.Quantity < 3)
            {
                urgentRestock.Add(part.Article);
                recommendations.Add($"🔄 {part.Name}: Залишилося критично мало ({part.Quantity} шт). Варто поповнити запаси.");
            }
        }

        if (recommendations.Count == 0)
        {
            recommendations.Add("✅ Склад укомплектовано. Термінових закупівель не потрібно.");
        }

        PurchasingRecommendations = new ObservableCollection<string>(recommendations);

        string reportLog = $"Звіт згенеровано о {DateTime.Now:HH:mm}. Унікальних деталей до замовлення: {urgentRestock.Count}";
        _reportHistory.Push(reportLog);
    }

    [RelayCommand]
    private void ExportReport()
    {
        try
        {
            string reportContent = $"--- АНАЛІТИЧНИЙ ЗВІТ СТО ---\nДата: {DateTime.Now:dd.MM.yyyy HH:mm}\n\n";
            reportContent += "РЕКОМЕНДАЦІЇ ЩОДО ЗАКУПІВЕЛЬ:\n";

            foreach (var rec in PurchasingRecommendations)
            {
                reportContent += rec + "\n";
            }

            File.WriteAllText("report.txt", reportContent);
            LoggerService.LogAction("Експортовано аналітичний звіт у файл report.txt");
        }
        catch (Exception ex)
        {
            LoggerService.LogAction($"Помилка експорту звіту: {ex.Message}");
        }
    }
}