using IceCreamSales.Services;

namespace IceCreamSales.Endpoints;

public static class ReportEndpoints
{
    public static void MapReportEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/reports");

        group.MapGet("/total-sales",
            (IReportService reportService) =>
            {
                return Results.Ok(new
                {
                    TotalSales = reportService.GetTotalSales()
                });
            });

        group.MapGet("/monthly-sales",
            (IReportService reportService) =>
            {
                return Results.Ok(
                    reportService.GetMonthlySales());
            });

        group.MapGet("/popular-items",
            (IReportService reportService) =>
            {
                return Results.Ok(
                    reportService.GetMostPopularItems());
            });

        group.MapGet("/highest-revenue-items",
            (IReportService reportService) =>
            {
                return Results.Ok(
                    reportService.GetHighestRevenueItems());
            });

        group.MapGet("/growth",
            (IReportService reportService) =>
            {
                return Results.Ok(
                    reportService.GetMonthToMonthGrowth());
            });

        group.MapGet("/validation-errors",
            (IReportService reportService) =>
            {
                return Results.Ok(
                    reportService.GetValidationErrors());
            });

        group.MapGet("dashboard", async (IDashboardPdfService dashboardPdfService) =>
        {
            var pdfDocument = await dashboardPdfService.GenerateDashboardPdfAsync();

            return Results.File(
                pdfDocument,
                "application/pdf",
                $"dashboard-{Guid.NewGuid()}.pdf");
        });
    }
}
