using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Strategies;

namespace TaxCalculator.Domain.Entities;

public sealed class TaxItem
{
    public string Name { get; }
    public TaxItemType Type { get; }
    public TaxItemParameters Parameters { get; }

    public TaxItem(string name, TaxItemType type, TaxItemParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tax item name cannot be empty.", nameof(name));

        Name = name;
        Type = type;
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }
}
