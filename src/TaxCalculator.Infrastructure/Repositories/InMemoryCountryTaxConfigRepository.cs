using System.Collections.Concurrent;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Interfaces;

namespace TaxCalculator.Infrastructure.Repositories;

public sealed class InMemoryCountryTaxConfigRepository : ICountryTaxConfigRepository
{
    private readonly ConcurrentDictionary<string, CountryTaxConfig> _store = new();

    public Task<CountryTaxConfig?> GetByCountryCodeAsync(
         string countryCode, CancellationToken ct = default)
    {
        var key = countryCode.ToUpperInvariant();
        _store.TryGetValue(key, out var config);
        return Task.FromResult<CountryTaxConfig?>(config);
    }

    public Task UpsertAsync(CountryTaxConfig config, CancellationToken ct = default)
    {
        _store.AddOrUpdate(
            key: config.CountryCode, 
            addValue: config,
            updateValueFactory: (_, _) => config);

        return Task.CompletedTask;
    }
    public IReadOnlyCollection<string> GetStoredCountryCodes()
        => _store.Keys.ToList().AsReadOnly();
}
