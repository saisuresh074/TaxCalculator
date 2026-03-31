using TaxCalculator.Application.DTOs;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Application.Factories;
public interface ITaxItemFactory
{
    /// <summary>
    /// Creates a domain TaxItem from the given DTO.
    /// </summary>
    TaxItem Create(TaxItemDto dto);
}
