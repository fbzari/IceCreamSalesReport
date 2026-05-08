using IceCreamSales.DTOs;
using IceCreamSales.Models;

namespace IceCreamSales.Services;

public class ReportService : IReportService
{
    private readonly ISalesDataStore _store;

    public ReportService(ISalesDataStore store)
    {
        _store = store;
    }

    private IEnumerable<SalesRecord> ValidRecords =>
        _store.Records.Where(x =>
            x.Date != null &&
            x.Quantity > 0 &&
            x.UnitPrice >= 0 &&
            x.TotalPrice >= 0 &&
            (x.UnitPrice * x.Quantity) == x.TotalPrice);

    public decimal GetTotalSales()
    {
        return ValidRecords.Sum(x => x.TotalPrice);
    }

    public IEnumerable<MonthlySalesDto> GetMonthlySales()
    {
        return ValidRecords
            .GroupBy(x => x.Date!.Value.ToString("yyyy-MM"))
            .Select(g => new MonthlySalesDto
            {
                Month = g.Key,
                TotalSales = g.Sum(x => x.TotalPrice)
            })
            .OrderBy(x => x.Month);
    }

    public IEnumerable<PopularItemDto> GetMostPopularItems()
    {
        var result = ValidRecords
            .GroupBy(x => x.Date!.Value.ToString("yyyy-MM"))
            .Select(monthGroup =>
            {
                var popularItem = monthGroup
                    .GroupBy(x => x.SKU)
                    .Select(g => new
                    {
                        SKU = g.Key,
                        TotalQuantity = g.Sum(x => x.Quantity),
                        MinOrders = g.Min(x => x.Quantity),
                        MaxOrders = g.Max(x => x.Quantity),
                        AverageOrders = g.Average(x => x.Quantity)
                    })
                    .OrderByDescending(x => x.TotalQuantity)
                    .First();

                return new PopularItemDto
                {
                    Month = monthGroup.Key,
                    SKU = popularItem.SKU,
                    TotalQuantitySold = popularItem.TotalQuantity,
                    MinOrders = popularItem.MinOrders,
                    MaxOrders = popularItem.MaxOrders,
                    AverageOrders = Math.Round(popularItem.AverageOrders, 2)
                };
            })
            .OrderBy(x => x.Month);

        return result;
    }

    public IEnumerable<RevenueItemDto> GetHighestRevenueItems()
    {
        return ValidRecords
            .GroupBy(x => x.Date!.Value.ToString("yyyy-MM"))
            .Select(monthGroup =>
            {
                var revenueItem = monthGroup
                    .GroupBy(x => x.SKU)
                    .Select(g => new
                    {
                        SKU = g.Key,
                        Revenue = g.Sum(x => x.TotalPrice)
                    })
                    .OrderByDescending(x => x.Revenue)
                    .First();

                return new RevenueItemDto
                {
                    Month = monthGroup.Key,
                    SKU = revenueItem.SKU,
                    Revenue = revenueItem.Revenue
                };
            })
            .OrderBy(x => x.Month);
    }

    public IEnumerable<ItemGrowthDto> GetMonthToMonthGrowth()
    {
        var monthlyRevenue = ValidRecords
            .GroupBy(x => new
            {
                Month = x.Date!.Value.ToString("yyyy-MM"),
                x.SKU
            })
            .Select(g => new
            {
                g.Key.Month,
                g.Key.SKU,
                Revenue = g.Sum(x => x.TotalPrice)
            })
            .OrderBy(x => x.Month)
            .ToList();

        var growthList = new List<ItemGrowthDto>();

        var groupedBySku = monthlyRevenue
            .GroupBy(x => x.SKU);

        foreach (var skuGroup in groupedBySku)
        {
            var ordered = skuGroup
                .OrderBy(x => x.Month)
                .ToList();

            for (int i = 1; i < ordered.Count; i++)
            {
                var previous = ordered[i - 1];
                var current = ordered[i];

                if (previous.Revenue == 0)
                    continue;

                decimal growth =
                    ((current.Revenue - previous.Revenue)
                    / previous.Revenue) * 100;

                growthList.Add(new ItemGrowthDto
                {
                    SKU = current.SKU,
                    PreviousMonth = previous.Month,
                    CurrentMonth = current.Month,
                    PreviousRevenue = previous.Revenue,
                    CurrentRevenue = current.Revenue,
                    GrowthPercentage = Math.Round(growth, 2)
                });
            }
        }

        return growthList;
    }

    public IEnumerable<ValidationErrorDto> GetValidationErrors()
    {
        var errors = new List<ValidationErrorDto>();

        foreach (var record in _store.Records)
        {
            if (record.UnitPrice * record.Quantity != record.TotalPrice)
            {
                errors.Add(new ValidationErrorDto
                {
                    RowNumber = record.RowNumber,
                    SKU = record.SKU,
                    RawDate = record.RawDate,
                    ErrorType = "INVALID_TOTAL",
                    Message =
                        "UnitPrice * Quantity does not equal TotalPrice"
                });
            }

            if (record.Quantity < 1)
            {
                errors.Add(new ValidationErrorDto
                {
                    RowNumber = record.RowNumber,
                    SKU = record.SKU,
                    RawDate = record.RawDate,
                    ErrorType = "INVALID_QUANTITY",
                    Message = "Quantity is less than 1"
                });
            }

            if (record.UnitPrice < 0)
            {
                errors.Add(new ValidationErrorDto
                {
                    RowNumber = record.RowNumber,
                    SKU = record.SKU,
                    RawDate = record.RawDate,
                    ErrorType = "INVALID_UNIT_PRICE",
                    Message = "UnitPrice is negative"
                });
            }

            if (record.TotalPrice < 0)
            {
                errors.Add(new ValidationErrorDto
                {
                    RowNumber = record.RowNumber,
                    SKU = record.SKU,
                    RawDate = record.RawDate,
                    ErrorType = "INVALID_TOTAL_PRICE",
                    Message = "TotalPrice is negative"
                });
            }

            if (record.Date == null)
            {
                errors.Add(new ValidationErrorDto
                {
                    RowNumber = record.RowNumber,
                    SKU = record.SKU,
                    RawDate = record.RawDate,
                    ErrorType = "INVALID_DATE",
                    Message = "Date format is malformed"
                });
            }
        }

        return errors;
    }
}
