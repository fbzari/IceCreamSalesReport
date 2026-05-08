using IceCreamSales.DTOs;

namespace IceCreamSales.Services;

public interface IReportService
{
    decimal GetTotalSales();

    IEnumerable<MonthlySalesDto> GetMonthlySales();

    IEnumerable<PopularItemDto> GetMostPopularItems();

    IEnumerable<RevenueItemDto> GetHighestRevenueItems();

    IEnumerable<ItemGrowthDto> GetMonthToMonthGrowth();
    IEnumerable<ValidationErrorDto> GetValidationErrors();
}
