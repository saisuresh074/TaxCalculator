using TaxCalculator.Application.DTOs;

namespace TaxCalculator.Application.Services;

public interface ITaxService
{
    Task<ConfigureTaxRuleResponse> ConfigureAsync(ConfigureTaxRuleRequest request, CancellationToken ct = default);
    Task<CalculateTaxResponse> CalculateAsync(CalculateTaxRequest request, CancellationToken ct = default);
}
