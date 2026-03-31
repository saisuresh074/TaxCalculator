using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Domain.Interfaces;

/// <summary>
/// TODO: Replace InMemory implementation with EF Core / SQL by implementing this interface.
/// </summary>
public interface ICountryTaxConfigRepository
{
    Task<CountryTaxConfig?> GetByCountryCodeAsync(string countryCode, CancellationToken ct = default);
    Task UpsertAsync(CountryTaxConfig config, CancellationToken ct = default);
}
