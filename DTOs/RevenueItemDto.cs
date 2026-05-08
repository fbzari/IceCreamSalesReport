namespace IceCreamSales.DTOs;

public class RevenueItemDto
{
    public string Month { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public decimal Revenue { get; set; }
}
