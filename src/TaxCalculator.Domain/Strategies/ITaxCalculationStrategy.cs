using TaxCalculator.Domain.Enums;

namespace TaxCalculator.Domain.Strategies;

/// <summary>
/// Strategy pattern interface for tax calculation.
/// Each tax item type owns exactly one strategy implementation.
/// </summary>
public interface ITaxCalculationStrategy
{
    TaxItemType TaxItemType { get; }
    decimal Calculate(TaxCalculationContext context);
}

public sealed record TaxCalculationContext(
    decimal GrossSalary,
    decimal TaxableBase,
    TaxItemParameters TaxItemParameters);
