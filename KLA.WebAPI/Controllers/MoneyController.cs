using System.Globalization;
using KLA.Domain.Services;
using KLA.Domain.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLA.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoneyController : ControllerBase
{
    private readonly ICurrencyToTextConverterService _currencyToTextConverterService;
    private readonly ICurrencyRangeValidator _currencyRangeValidator;
    private readonly ICurrencyParser _currencyParser;

    public MoneyController(
        ICurrencyToTextConverterService currencyToTextConverterService,
        ICurrencyRangeValidator currencyRangeValidator,
        ICurrencyParser parser)
    {
        _currencyToTextConverterService = currencyToTextConverterService ?? throw new ArgumentNullException(nameof(currencyToTextConverterService));
        _currencyRangeValidator = currencyRangeValidator ?? throw new ArgumentNullException(nameof(currencyRangeValidator));
        _currencyParser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    [HttpGet(@"text/{money:required}", Name = "text")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Get(string money)
    {
        var parseResult = _currencyParser.Parse(money);
        if (!parseResult.Parsed)
        {
            return await Task.FromResult<IActionResult>(BadRequest(parseResult.Message));
        }
        
        var validationResult = _currencyRangeValidator.Validate(parseResult.ParsedValue!.Value);
        if (validationResult.Status != CurrencyValidationStatus.Valid)
        {
            return await Task.FromResult<IActionResult>(BadRequest(validationResult.Message));
        }

        try
        {
            var text = _currencyToTextConverterService.ConvertToText(parseResult.ParsedValue!.Value);
            return await Task.FromResult<IActionResult>(Ok(text));
        }
        catch (ArgumentOutOfRangeException outOfRangeException)
        {
            return await Task.FromResult<IActionResult>(BadRequest(outOfRangeException.Message));
        }
        catch (Exception)
        {
            return await Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError));
        }
    }
}