using TaxCalculator.Application.DTOs;
using TaxCalculator.Application.Factories;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Strategies;

namespace TaxCalculator.Application.Services;

/// <summary>
/// Application service that orchestrates tax configuration and calculation.
/// </summary>
public sealed class TaxService : ITaxService
{
    private readonly ICountryTaxConfigRepository _repository;
    private readonly ITaxItemFactory _taxItemFactory;
    private readonly ITaxCalculationStrategyRegistry _strategyRegistry;

    public TaxService(
        ICountryTaxConfigRepository repository,
        ITaxItemFactory taxItemFactory,
        ITaxCalculationStrategyRegistry strategyRegistry)
    {
        _repository = repository;
        _taxItemFactory = taxItemFactory;
        _strategyRegistry = strategyRegistry;
    }

    public async Task<ConfigureTaxRuleResponse> ConfigureAsync(
        ConfigureTaxRuleRequest request, CancellationToken ct = default)
    {
        var domainItems = request.TaxItems
            .Select(_taxItemFactory.Create)
            .ToList();

        var config = new CountryTaxConfig(request.CountryCode, domainItems);

        await _repository.UpsertAsync(config, ct);

        return new ConfigureTaxRuleResponse
        {
            CountryCode = config.CountryCode,
            Message = $"Tax configuration for '{config.CountryCode}' saved successfully."
        };
    }

    public async Task<CalculateTaxResponse> CalculateAsync(
        CalculateTaxRequest request, CancellationToken ct = default)
    {
        var config = await _repository.GetByCountryCodeAsync(request.CountryCode, ct)
            ?? throw new CountryConfigNotFoundException(request.CountryCode);
        // TODO - resolve repo or  call External provider integration

        var result = config.Calculate(request.GrossSalary, _strategyRegistry);

        //TODO - Calculate credits applied and return in the response

        //if (request.EmployeeId is not null)
        //credits = await _taxCreditService.GetCreditAmountAsync(request.EmployeeId, ct);
        //decimal taxesAfterCredits = Math.Max(0m, result.TotalTaxes - credits);

        return new CalculateTaxResponse
        {
            GrossSalary = result.GrossSalary,
            TaxableBase = result.TaxableBase,
            TotalTaxes  = result.TotalTaxes,
            NetSalary   = result.NetSalary,
            Breakdown   = result.Breakdown
                .Select(b => new TaxItemBreakdownDto
                {
                    Name   = b.Name,
                    Type   = b.Type.ToString(),
                    Amount = b.Amount
                })
                .ToList()
        };
    }
}
