using IceCreamSales.Models;

namespace IceCreamSales.Services;

public class CsvParserService
{
    public List<SalesRecord> Parse(string csvData)
    {
        var records = new List<SalesRecord>();

        using var reader = new StringReader(csvData);

        var lines = reader.ReadToEnd()
            .Split(Environment.NewLine)
            .Skip(1)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        int rowNumber = 1;

        foreach (var line in lines)
        {
            var columns = line.Split(',');

            DateTime parsedDate;

            DateTime? date = DateTime.TryParse(
                columns[0],
                out parsedDate)
                ? parsedDate
                : null;

            records.Add(new SalesRecord
            {
                RowNumber = rowNumber++,
                RawDate = columns[0],
                Date = date ?? DateTime.MinValue,
                SKU = columns[1],
                UnitPrice = decimal.Parse(columns[2]),
                Quantity = int.Parse(columns[3]),
                TotalPrice = decimal.Parse(columns[4])
            });
        }

        return records;
    }
}
