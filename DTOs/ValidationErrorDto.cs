namespace IceCreamSales.DTOs;

public class ValidationErrorDto
{
    public int RowNumber { get; set; }

    public string ErrorType { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string? SKU { get; set; }

    public string? RawDate { get; set; }
}
