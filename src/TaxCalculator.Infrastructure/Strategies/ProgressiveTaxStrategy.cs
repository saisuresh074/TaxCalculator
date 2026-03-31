using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Strategies;

namespace TaxCalculator.Infrastructure.Strategies;

/// <summary>
/// Strategy for Progressive
/// </summary>
public sealed class ProgressiveTaxStrategy : ITaxCalculationStrategy
{
    public TaxItemType TaxItemType => TaxItemType.Progressive;

    public decimal Calculate(TaxCalculationContext context)
    {
        var brackets = context.TaxItemParameters.Brackets;

        if (brackets is null || brackets.Count == 0)
            throw new InvalidTaxConfigurationException(
                "ProgressiveTaxStrategy requires at least one bracket in TaxItemParameters.Brackets.");

        // Validate: only the last bracket may have a null threshold
        for (int i = 0; i < brackets.Count - 1; i++)
        {
            if (brackets[i].Threshold is null)
                throw new InvalidTaxConfigurationException(
                    $"Bracket at index {i} has a null threshold, but only the last bracket may be open-ended.");
        }

        decimal taxableBase = context.TaxableBase;
        decimal totalTax = 0m;
        decimal previousThreshold = 0m;

        foreach (var bracket in brackets)
        {
            if (taxableBase <= previousThreshold) break;

            decimal upperBound = bracket.Threshold.HasValue
                ? Math.Min(bracket.Threshold.Value, taxableBase)
                : taxableBase;

            decimal taxableInBracket = upperBound - previousThreshold;
            totalTax += taxableInBracket * (bracket.Rate / 100m);

            if (bracket.Threshold.HasValue)
                previousThreshold = bracket.Threshold.Value;
            else
                break;
        }

        return totalTax;
    }
}
