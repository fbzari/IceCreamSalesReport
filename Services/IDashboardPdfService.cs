namespace IceCreamSales.Services;

public interface IDashboardPdfService
{
    Task<byte[]> GenerateDashboardPdfAsync();
}
