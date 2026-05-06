using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AutoService_Master.Models;

namespace AutoService_Master.Services;

public class JsonDataService
{
    private readonly string _clientsFilePath = "clients_data.json";
    private readonly string _ordersFilePath = "orders_data.json";
    private readonly string _partsFilePath = "parts_data.json";

    public void SaveClients(IEnumerable<Client> clients)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(clients, options);
            File.WriteAllText(_clientsFilePath, jsonString);
            LoggerService.LogAction("Успішно збережено дані клієнтів");
        }
        catch (Exception ex)
        {
            LoggerService.LogAction($"Помилка збереження клієнтів: {ex.Message}");
        }
    }

    public List<Client> LoadClients()
    {
        try
        {
            if (!File.Exists(_clientsFilePath)) return new List<Client>();
            string jsonString = File.ReadAllText(_clientsFilePath);
            var clients = JsonSerializer.Deserialize<List<Client>>(jsonString);
            return clients ?? new List<Client>();
        }
        catch (Exception ex)
        {
            LoggerService.LogAction($"Помилка завантаження клієнтів: {ex.Message}");
            return new List<Client>();
        }
    }

    public void SaveOrders(IEnumerable<Order> orders)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(orders, options);
            File.WriteAllText(_ordersFilePath, jsonString);
            LoggerService.LogAction("Успішно збережено базу замовлень");
        }
        catch (Exception ex)
        {
            LoggerService.LogAction($"Помилка збереження замовлень: {ex.Message}");
        }
    }

    public List<Order> LoadOrders()
    {
        try
        {
            if (!File.Exists(_ordersFilePath)) return new List<Order>();
            string jsonString = File.ReadAllText(_ordersFilePath);
            var orders = JsonSerializer.Deserialize<List<Order>>(jsonString);
            return orders ?? new List<Order>();
        }
        catch (Exception ex)
        {
            LoggerService.LogAction($"Помилка завантаження замовлень: {ex.Message}");
            return new List<Order>();
        }
    }

    public void SaveParts(IEnumerable<Part> parts)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(parts, options);
            File.WriteAllText(_partsFilePath, jsonString);
            LoggerService.LogAction("Успішно оновлено стан складу");
        }
        catch (Exception ex)
        {
            LoggerService.LogAction($"Помилка збереження складу: {ex.Message}");
        }
    }

    public List<Part> LoadParts()
    {
        try
        {
            if (!File.Exists(_partsFilePath)) return new List<Part>();
            string jsonString = File.ReadAllText(_partsFilePath);
            var parts = JsonSerializer.Deserialize<List<Part>>(jsonString);
            return parts ?? new List<Part>();
        }
        catch (Exception ex)
        {
            LoggerService.LogAction($"Помилка завантаження складу: {ex.Message}");
            return new List<Part>();
        }
    }
}