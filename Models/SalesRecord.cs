namespace IceCreamSales.Models;

public class SalesRecord
{
    public string RawDate { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string SKU { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int RowNumber { get; set; }
}
