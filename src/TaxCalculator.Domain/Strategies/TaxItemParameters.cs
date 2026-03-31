namespace TaxCalculator.Domain.Strategies;

public sealed class TaxItemParameters
{
    public decimal? FixedAmount { get; init; }

    public decimal? FlatRate { get; init; }

    public IReadOnlyList<ProgressiveBracketParameter>? Brackets { get; init; }
}

/// <summary>A single bracket in a progressive tax schedule.</summary>
public sealed record ProgressiveBracketParameter(decimal? Threshold, decimal Rate);
