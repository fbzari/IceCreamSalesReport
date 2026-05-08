using IceCreamSales.Models;

namespace IceCreamSales.Services;

public interface ISalesDataStore
{
    IReadOnlyList<SalesRecord> Records { get; }
}
