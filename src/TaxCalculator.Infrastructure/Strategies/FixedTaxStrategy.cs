using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Strategies;

namespace TaxCalculator.Infrastructure.Strategies;

/// <summary>
/// Strategy for fixed amount
/// </summary>
public sealed class FixedTaxStrategy : ITaxCalculationStrategy
{
    public TaxItemType TaxItemType => TaxItemType.Fixed;

    public decimal Calculate(TaxCalculationContext context)
    {
        if (context.TaxItemParameters.FixedAmount is null)
            throw new InvalidTaxConfigurationException(
                "FixedTaxStrategy requires TaxItemParameters.FixedAmount to be set.");

        return context.TaxItemParameters.FixedAmount.Value;
    }
}
