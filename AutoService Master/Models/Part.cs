using System;
using System.Collections.Generic;
using System.Text;

namespace AutoService_Master.Models;

public class Part
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}