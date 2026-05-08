using IceCreamSales.Endpoints;
using IceCreamSales.Services;

/// This Repo Contains the Sales Report Assessment
/// 
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

///1) What was the most complex part of the assignment for you personally and why?
///    ans : For Generate Report as pdf for crucial part. integrate separate razor 
///    engine in this minimal API project
///2) Describe a bug you expect to hit while implementing this and how you would debug it.
/// ans : a bug that was facing while generate PDF using razor html. and generate pdf using thirt party library.
///3) Does your solution handle larger data sets without any performance implications?
/// ans : yes we have separate csv file. you can use large data also.
