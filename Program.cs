using IceCreamSales.Endpoints;
using IceCreamSales.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// DI

builder.Services.AddSingleton<ISalesDataStore, SalesDataStore>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IDashboardPdfService, DashboardPdfService>();

builder.Services.AddRazorTemplating();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapReportEndpoints();
app.Run();
