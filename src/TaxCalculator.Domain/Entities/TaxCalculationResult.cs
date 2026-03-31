using TaxCalculator.Domain.Enums;

namespace TaxCalculator.Domain.Entities;

public sealed record TaxItemResult(string Name, TaxItemType Type, decimal Amount);

public sealed record TaxCalculationResult(
    decimal GrossSalary,
    decimal TaxableBase,
    decimal TotalTaxes,
    decimal NetSalary,
    IReadOnlyList<TaxItemResult> Breakdown);
