namespace IceCreamSales.DTOs;

public class PopularItemDto
{
    public string Month { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public int TotalQuantitySold { get; set; }

    public int MinOrders { get; set; }

    public int MaxOrders { get; set; }

    public double AverageOrders { get; set; }
}
