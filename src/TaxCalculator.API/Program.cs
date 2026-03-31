using System.Reflection;
using TaxCalculator.API.Middleware;
using TaxCalculator.Application.Factories;
using TaxCalculator.Application.Services;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Strategies;
using TaxCalculator.Infrastructure.Credits;
using TaxCalculator.Infrastructure.Repositories;
using TaxCalculator.Infrastructure.Strategies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title       = "Tax Calculator API",
        Version     = "v1",
        Description =
            "Configures and calculates taxes per country"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

builder.Services.AddSingleton<ITaxCalculationStrategy, FixedTaxStrategy>();
builder.Services.AddSingleton<ITaxCalculationStrategy, FlatRateTaxStrategy>();
builder.Services.AddSingleton<ITaxCalculationStrategy, ProgressiveTaxStrategy>();

builder.Services.AddSingleton<ITaxCalculationStrategyRegistry, TaxCalculationStrategyRegistry>();


builder.Services.AddSingleton<ICountryTaxConfigRepository, InMemoryCountryTaxConfigRepository>();
builder.Services.AddScoped<ITaxItemFactory, TaxItemFactory>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<ITaxCreditService, TaxCreditService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tax Calculator API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Exposed for integration testing
public partial class Program { }
