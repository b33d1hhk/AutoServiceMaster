using System;
using System.Collections.Generic;
using System.Text;

namespace AutoService_Master.Models;

public enum OrderStatus
{
    Pending,
    InProgress,
    Completed,
    Paid
}

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ClientId { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}