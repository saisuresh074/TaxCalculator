using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Application.DTOs;
using TaxCalculator.Application.Services;

namespace TaxCalculator.API.Controllers;

[ApiController]
[Route("api/tax")]
[Produces("application/json")]
public sealed class TaxController : ControllerBase
{
    private readonly ITaxService _taxService;

    public TaxController(ITaxService taxService)
    {
        _taxService = taxService;
    }

    /// <summary>
    /// Configure tax rules for a country.
    /// Tax item types: 1 = Fixed, 2 = FlatRate, 3 = Progressive.
    /// </summary>
    [HttpPost("configure")]
    [ProducesResponseType(typeof(ConfigureTaxRuleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Configure(
        [FromBody] ConfigureTaxRuleRequest request,
        CancellationToken ct)
    {
        var response = await _taxService.ConfigureAsync(request, ct);
        return Ok(response);
    }

    /// <summary>
    /// Calculate tax for a given country and annual gross salary.
    /// </summary>
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(CalculateTaxResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Calculate(
        [FromBody] CalculateTaxRequest request,
        CancellationToken ct)
    {
        var response = await _taxService.CalculateAsync(request, ct);
        return Ok(response);
    }
}
