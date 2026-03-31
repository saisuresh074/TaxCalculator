using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Strategies;

namespace TaxCalculator.Infrastructure.Strategies;

public sealed class TaxCalculationStrategyRegistry : ITaxCalculationStrategyRegistry
{
    private readonly IReadOnlyDictionary<TaxItemType, ITaxCalculationStrategy> _strategies;

    public TaxCalculationStrategyRegistry(IEnumerable<ITaxCalculationStrategy> strategies)
    {
        var dict = new Dictionary<TaxItemType, ITaxCalculationStrategy>();
        foreach (var strategy in strategies)
            dict[strategy.TaxItemType] = strategy;
        _strategies = dict;
    }

    public ITaxCalculationStrategy Resolve(TaxItemType taxItemType)
    {
        if (_strategies.TryGetValue(taxItemType, out var strategy))
            return strategy;

        throw new NoStrategyRegisteredException(taxItemType.ToString());
    }

    public bool IsRegistered(TaxItemType taxItemType)
        => _strategies.ContainsKey(taxItemType);
}
