using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Strategies;

namespace TaxCalculator.Infrastructure.Strategies;

/// <summary>
/// Strategy for FlatRate/>.
/// </summary>
public sealed class FlatRateTaxStrategy : ITaxCalculationStrategy
{
    public TaxItemType TaxItemType => TaxItemType.FlatRate;

    public decimal Calculate(TaxCalculationContext context)
    {
        if (context.TaxItemParameters.FlatRate is null)
            throw new InvalidTaxConfigurationException(
                "FlatRateTaxStrategy requires TaxItemParameters.FlatRate to be set.");

        decimal rate = context.TaxItemParameters.FlatRate.Value;

        if (rate < 0 || rate > 100)
            throw new InvalidTaxConfigurationException(
                $"FlatRate must be between 0 and 100, but was {rate}.");

        return context.TaxableBase * (rate / 100m);
    }
}
