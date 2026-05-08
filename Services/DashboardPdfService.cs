using IceCreamSales.DTOs;
using Razor.Templating.Core;

namespace IceCreamSales.Services;
public class DashboardPdfService : IDashboardPdfService
{
    private readonly IReportService _reportService;
    public DashboardPdfService(
        IReportService reportService,
        IWebHostEnvironment env)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> GenerateDashboardPdfAsync()
    {
        var model = new DashboardReportDto
        {
            TotalSales = _reportService.GetTotalSales(),
            MonthlySales = _reportService.GetMonthlySales(),
            PopularItems = _reportService.GetMostPopularItems(),
            HighestRevenueItems =
                _reportService.GetHighestRevenueItems(),
            GrowthItems =
                _reportService.GetMonthToMonthGrowth(),
            ValidationErrors =
                _reportService.GetValidationErrors()
        };

        string html = await RazorTemplateEngine.RenderAsync(
                        "Views/Dashboard.cshtml",
                        model);

        var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
        var pdfBytes = htmlToPdf.GeneratePdf(html);

        return pdfBytes;
    }
}
