using IceCreamSales.Models;
using System.Globalization;

namespace IceCreamSales.Services;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class SalesDataStore : ISalesDataStore
{
    public IReadOnlyList<SalesRecord> Records { get; }

    public SalesDataStore(IWebHostEnvironment env)
    {
        var filePath = Path.Combine(
            env.ContentRootPath,
            "Data",
            "sales.csv");

        var records = new List<SalesRecord>();

        var config = new CsvConfiguration(
            CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            HeaderValidated = null,
            BadDataFound = null
        };

        using var reader = new StreamReader(filePath);

        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();

        int rowNumber = 1;

        while (csv.Read())
        {
            try
            {
                var rawDate = csv.GetField("Date");

                DateTime parsedDate;

                DateTime? date =
                    DateTime.TryParse(
                        rawDate,
                        out parsedDate)
                    ? parsedDate
                    : null;

                var record = new SalesRecord
                {
                    RowNumber = rowNumber++,

                    RawDate = rawDate,

                    Date = date,

                    SKU = csv.GetField("SKU") ?? string.Empty,

                    UnitPrice =
                        decimal.TryParse(
                            csv.GetField("UnitPrice"),
                            out var unitPrice)
                        ? unitPrice
                        : -1,

                    Quantity =
                        int.TryParse(
                            csv.GetField("Quantity"),
                            out var quantity)
                        ? quantity
                        : -1,

                    TotalPrice =
                        decimal.TryParse(
                            csv.GetField("TotalPrice"),
                            out var totalPrice)
                        ? totalPrice
                        : -1
                };

                records.Add(record);
            }
            catch
            {
                // Never crash application startup
                // Optional: log error using Serilog
            }
        }

        Records = records;
    }
}
