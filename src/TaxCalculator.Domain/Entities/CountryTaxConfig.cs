using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Strategies;

namespace TaxCalculator.Domain.Entities;

public sealed class CountryTaxConfig
{
    public string CountryCode { get; }
    public IReadOnlyList<TaxItem> TaxItems { get; }

    public CountryTaxConfig(string countryCode, IEnumerable<TaxItem> taxItems)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Country code cannot be empty.", nameof(countryCode));

        CountryCode = countryCode.ToUpperInvariant();

        var list = taxItems?.ToList()
            ?? throw new ArgumentNullException(nameof(taxItems));

        if (list.Count == 0)
            throw new InvalidTaxConfigurationException(
                "At least one tax item must be provided.");

        int progressiveCount = list.Count(t => t.Type == TaxItemType.Progressive);
        if (progressiveCount > 1)
            throw new InvalidTaxConfigurationException(
                "Only one progressive tax item is allowed per country.");

        TaxItems = list.AsReadOnly();
    }

    /// <summary>
    /// Calculates the full tax breakdown
    /// </summary>
    public TaxCalculationResult Calculate(decimal grossSalary, ITaxCalculationStrategyRegistry strategyRegistry)
    {
        if (grossSalary < 0)
            throw new ArgumentException("Gross salary cannot be negative.", nameof(grossSalary));

        var fixedResults = TaxItems
            .Where(t => t.Type == TaxItemType.Fixed)
            .Select(t =>
            {
                var strategy = strategyRegistry.Resolve(t.Type);
                var ctx = new TaxCalculationContext(grossSalary, 0m, t.Parameters);
                return (Item: t, Amount: strategy.Calculate(ctx));
            })
            .ToList();

        decimal fixedTotal = fixedResults.Sum(r => r.Amount);

        decimal taxableBase = Math.Max(0m, grossSalary - fixedTotal);

        var fixedByItem = fixedResults.ToDictionary(r => r.Item, r => r.Amount);
        var breakdown = new List<TaxItemResult>();

        foreach (var item in TaxItems)
        {
            decimal amount;

            if (fixedByItem.TryGetValue(item, out var cached))
            {
                amount = cached;
            }
            else
            {
                var strategy = strategyRegistry.Resolve(item.Type);
                var context = new TaxCalculationContext(grossSalary, taxableBase, item.Parameters);
                amount = strategy.Calculate(context);
            }

            breakdown.Add(new TaxItemResult(item.Name, item.Type, amount));
        }

        decimal totalTaxes = breakdown.Sum(b => b.Amount);
        decimal netSalary = grossSalary - totalTaxes;

        return new TaxCalculationResult(
            GrossSalary: grossSalary,
            TaxableBase: taxableBase,
            TotalTaxes: totalTaxes,
            NetSalary: netSalary,
            Breakdown: breakdown);
    }
}
