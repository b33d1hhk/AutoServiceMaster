using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AutoService_Master.Models;

namespace AutoService_Master.Services;

public class JsonDataService
{
    private readonly string _filePath = "clients_data.json";
    
    public void SaveClients(IEnumerable<Client> clients)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(clients, options);
        File.WriteAllText(_filePath, jsonString);
    }
    
    public List<Client> LoadClients()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Client>();
        }
        string jsonString = File.ReadAllText(_filePath);
        var clients = JsonSerializer.Deserialize<List<Client>>(jsonString);
        return clients ?? new List<Client>();
    }
}