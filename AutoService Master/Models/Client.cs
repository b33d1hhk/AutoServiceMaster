using System;

namespace AutoService_Master.Models;

public class Client
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string FullName { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string CarInfo { get; set; } = string.Empty;
    
    public string LicensePlate { get; set; } = string.Empty;
    
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}