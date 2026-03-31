using TaxCalculator.Domain.Enums;

namespace TaxCalculator.Domain.Strategies;

/// <summary>
/// Implementations should be registered via DI and resolved here.
/// </summary>
public interface ITaxCalculationStrategyRegistry
{
    /// <summary>
    /// Returns the strategy registered for the given taxItemType/>.
    /// </summary>
    ITaxCalculationStrategy Resolve(TaxItemType taxItemType);

    /// <summary>Returns true if a strategy is registered for the given type.</summary>
    bool IsRegistered(TaxItemType taxItemType);
}
