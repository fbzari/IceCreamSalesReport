namespace IceCreamSales.DTOs;

public class ItemGrowthDto
{
    public string SKU { get; set; } = string.Empty;

    public string PreviousMonth { get; set; } = string.Empty;

    public string CurrentMonth { get; set; } = string.Empty;

    public decimal PreviousRevenue { get; set; }

    public decimal CurrentRevenue { get; set; }

    public decimal GrowthPercentage { get; set; }
}
