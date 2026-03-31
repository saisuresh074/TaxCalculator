using TaxCalculator.Application.DTOs;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Enums;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Strategies;

namespace TaxCalculator.Application.Factories;

public sealed class TaxItemFactory : ITaxItemFactory
{
    private readonly ITaxCalculationStrategyRegistry _strategyRegistry;

    public TaxItemFactory(ITaxCalculationStrategyRegistry strategyRegistry)
    {
        _strategyRegistry = strategyRegistry;
    }

    public TaxItem Create(TaxItemDto dto)
    {
        if (!_strategyRegistry.IsRegistered(dto.Type))
            throw new NoStrategyRegisteredException(dto.Type.ToString());

        var parameters = dto.Type switch
        {
            TaxItemType.Fixed      => BuildFixedParameters(dto),
            TaxItemType.FlatRate   => BuildFlatRateParameters(dto),
            TaxItemType.Progressive => BuildProgressiveParameters(dto),
            _ => throw new InvalidTaxConfigurationException(
                     $"Unknown tax item type '{dto.Type}'.")
        };

        return new TaxItem(dto.Name, dto.Type, parameters);
    }
    private static TaxItemParameters BuildFixedParameters(TaxItemDto dto)
    {
        if (dto.Amount is null)
            throw new InvalidTaxConfigurationException(
                $"Tax item '{dto.Name}': 'amount' is required for Fixed type.");

        if (dto.Amount < 0)
            throw new InvalidTaxConfigurationException(
                $"Tax item '{dto.Name}': 'amount' must be non-negative.");

        return new TaxItemParameters { FixedAmount = dto.Amount };
    }

    private static TaxItemParameters BuildFlatRateParameters(TaxItemDto dto)
    {
        if (dto.Rate is null)
            throw new InvalidTaxConfigurationException(
                $"Tax item '{dto.Name}': 'rate' is required for FlatRate type.");

        if (dto.Rate is < 0 or > 100)
            throw new InvalidTaxConfigurationException(
                $"Tax item '{dto.Name}': 'rate' must be between 0 and 100.");

        return new TaxItemParameters { FlatRate = dto.Rate };
    }

    private static TaxItemParameters BuildProgressiveParameters(TaxItemDto dto)
    {
        if (dto.Brackets is null || dto.Brackets.Count == 0)
            throw new InvalidTaxConfigurationException(
                $"Tax item '{dto.Name}': 'brackets' are required for Progressive type.");

        var brackets = dto.Brackets
            .Select(b => new ProgressiveBracketParameter(b.Threshold, b.Rate))
            .ToList()
            .AsReadOnly();

        return new TaxItemParameters { Brackets = brackets };
    }
}
