namespace IceCreamSales.DTOs;

public class DashboardReportDto
{
    public decimal TotalSales { get; set; }

    public IEnumerable<MonthlySalesDto> MonthlySales { get; set; }
        = Enumerable.Empty<MonthlySalesDto>();

    public IEnumerable<PopularItemDto> PopularItems { get; set; }
        = Enumerable.Empty<PopularItemDto>();

    public IEnumerable<RevenueItemDto> HighestRevenueItems { get; set; }
        = Enumerable.Empty<RevenueItemDto>();

    public IEnumerable<ItemGrowthDto> GrowthItems { get; set; }
        = Enumerable.Empty<ItemGrowthDto>();

    public IEnumerable<ValidationErrorDto> ValidationErrors { get; set; }
        = Enumerable.Empty<ValidationErrorDto>();
}
