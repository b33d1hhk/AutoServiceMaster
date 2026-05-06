using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoService_Master.Models;
using AutoService_Master.Services;

namespace AutoService_Master.ViewModels;

public partial class ClientsViewModel : ViewModelBase
{
    private readonly JsonDataService _dataService;
    private List<Client> _allClients;

    [ObservableProperty]
    private ObservableCollection<Client> _clients;

    [ObservableProperty]
    private string _searchQuery = string.Empty;

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
        _allClients = _dataService.LoadClients();
        FilterClients();
    }

    partial void OnSearchQueryChanged(string value)
    {
        FilterClients();
    }

    private void FilterClients()
    {
        List<Client> resultList;

        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            resultList = new List<Client>(_allClients);
        }
        else
        {
            var query = SearchQuery.ToLower();
            resultList = new List<Client>();

            foreach (var c in _allClients)
            {
                string nameLower = c.FullName.ToLower();

                bool exactMatch = nameLower.Contains(query) || c.PhoneNumber.Contains(query);

                bool fuzzyMatch = false;
                if (Math.Abs(nameLower.Length - query.Length) <= 3)
                {
                    int distance = ComputeLevenshteinDistance(nameLower, query);
                    fuzzyMatch = distance <= 2;
                }

                if (exactMatch || fuzzyMatch)
                {
                    resultList.Add(c);
                }
            }
        }

        if (resultList.Count > 0)
        {
            QuickSortClients(resultList, 0, resultList.Count - 1);
        }

        Clients = new ObservableCollection<Client>(resultList);
    }

    private int ComputeLevenshteinDistance(string s, string t)
    {
        if (string.IsNullOrEmpty(s)) return string.IsNullOrEmpty(t) ? 0 : t.Length;
        if (string.IsNullOrEmpty(t)) return s.Length;

        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        for (int i = 0; i <= n; d[i, 0] = i++) { }
        for (int j = 0; j <= m; d[0, j] = j++) { }

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        return d[n, m];
    }

    private void QuickSortClients(List<Client> list, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(list, left, right);
            QuickSortClients(list, left, pivotIndex - 1);
            QuickSortClients(list, pivotIndex + 1, right);
        }
    }

    private int Partition(List<Client> list, int left, int right)
    {
        var pivot = list[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (CompareClients(list[j], pivot) <= 0)
            {
                i++;
                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        var temp1 = list[i + 1];
        list[i + 1] = list[right];
        list[right] = temp1;

        return i + 1;
    }

    private int CompareClients(Client a, Client b)
    {
        int nameComparison = string.Compare(a.FullName, b.FullName, StringComparison.OrdinalIgnoreCase);
        if (nameComparison != 0) return nameComparison;

        return string.Compare(a.PhoneNumber, b.PhoneNumber, StringComparison.OrdinalIgnoreCase);
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
        if (string.IsNullOrWhiteSpace(NewFullName)) return;

        if (_editingClient == null)
        {
            var newClient = new Client { FullName = NewFullName, PhoneNumber = NewPhone, CarInfo = NewCarInfo };
            _allClients.Add(newClient);
        }
        else
        {
            _editingClient.FullName = NewFullName;
            _editingClient.PhoneNumber = NewPhone;
            _editingClient.CarInfo = NewCarInfo;
            _editingClient = null;
        }

        _dataService.SaveClients(_allClients);
        FilterClients();

        NewFullName = string.Empty;
        NewPhone = string.Empty;
        NewCarInfo = string.Empty;
    }

    [RelayCommand]
    private void DeleteClient(Client clientToDelete)
    {
        if (clientToDelete != null)
        {
            _allClients.Remove(clientToDelete);
            _dataService.SaveClients(_allClients);
            FilterClients();
        }
    }
}