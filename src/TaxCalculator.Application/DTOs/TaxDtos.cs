using System.ComponentModel.DataAnnotations;
using TaxCalculator.Domain.Enums;

namespace TaxCalculator.Application.DTOs;
public sealed class ConfigureTaxRuleRequest
{
    /// <example>DE</example>
    [Required, MinLength(2), MaxLength(3)]
    public string CountryCode { get; set; } = string.Empty;

    [Required, MinLength(1)]
    public List<TaxItemDto> TaxItems { get; set; } = new();
}

public sealed class TaxItemDto
{
    /// <example>IncomeTax</example>
    [Required, MinLength(1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public TaxItemType Type { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Rate { get; set; }

    public List<ProgressiveBracketDto>? Brackets { get; set; }
}

public sealed class ProgressiveBracketDto
{
    public decimal? Threshold { get; set; }

    [Range(0, 100)]
    public decimal Rate { get; set; }
}
public sealed class CalculateTaxRequest
{
    [Required]
    public string CountryCode { get; set; } = string.Empty;

    [Required, Range(0, double.MaxValue)]
    public decimal GrossSalary { get; set; }

    // TODO — Calculate Tax Credits based on Employee Id
    //public string? EmployeeId { get; set; }
}
public sealed class CalculateTaxResponse
{
    public decimal GrossSalary { get; set; }
    public decimal TaxableBase { get; set; }
    public decimal TotalTaxes { get; set; }
    public decimal NetSalary { get; set; }
    public List<TaxItemBreakdownDto> Breakdown { get; set; } = new();
}

public sealed class TaxItemBreakdownDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public sealed class ConfigureTaxRuleResponse
{
    public string CountryCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
