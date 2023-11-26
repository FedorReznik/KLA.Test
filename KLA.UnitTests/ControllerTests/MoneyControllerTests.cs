using KLA.Domain.Services;
using KLA.Domain.Shared.Services;
using KLA.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace KLA.Test.UnitTests.ControllerTests;

public class MoneyControllerTests
{
    private readonly ICurrencyParser _parser = Substitute.For<ICurrencyParser>();
    private readonly ICurrencyRangeValidator _validator = Substitute.For<ICurrencyRangeValidator>();
    private readonly ICurrencyToTextConverterService _converter = Substitute.For<ICurrencyToTextConverterService>();
    
    [Fact]
    public async Task ShouldReturnBadRequestOnFailedParsing()
    {
        _parser.Parse(string.Empty)
            .ReturnsForAnyArgs(new CurrencyParseResult(null, false, string.Empty));
        var sut = Create();
        
        var response = (ObjectResult) await sut.Get(string.Empty);
        
        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode!.Value);    
    }
    
    [Fact]
    public async Task ShouldReturnBadRequestTooBigValidationResult()
    {
        _parser.Parse(string.Empty)
            .ReturnsForAnyArgs(new CurrencyParseResult(42, true, string.Empty));
        _validator.Validate(42).ReturnsForAnyArgs(new CurrencyValidationResult(CurrencyValidationStatus.TooBig, string.Empty));
        
        var sut = Create();
        
        var response = (ObjectResult) await sut.Get(string.Empty);
        
        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode!.Value);    
    }
    
    [Fact]
    public async Task ShouldReturnBadRequestTooSmallValidationResult()
    {
        _parser.Parse(string.Empty)
            .ReturnsForAnyArgs(new CurrencyParseResult(42, true, string.Empty));
        _validator.Validate(42).ReturnsForAnyArgs(new CurrencyValidationResult(CurrencyValidationStatus.TooSmall, string.Empty));
        
        var sut = Create();
        
        var response = (ObjectResult) await sut.Get(string.Empty);
        
        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode!.Value);    
    }
    
    [Fact]
    public async Task ShouldConvertIfParsedAndValidated()
    {
        _parser.Parse(string.Empty)
            .ReturnsForAnyArgs(new CurrencyParseResult(42, true, string.Empty));
        _validator.Validate(42).ReturnsForAnyArgs(new CurrencyValidationResult(CurrencyValidationStatus.Valid, string.Empty));
        
        var sut = Create();
        
        var response = (ObjectResult) await sut.Get(string.Empty);
        
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode!.Value);    
    }
    
    [Fact]
    public async Task ShouldReturnBadRequestOnOutOfRange()
    {
        _parser.Parse(string.Empty)
            .ReturnsForAnyArgs(new CurrencyParseResult(42, true, string.Empty));
        _validator.Validate(42).ReturnsForAnyArgs(new CurrencyValidationResult(CurrencyValidationStatus.Valid, string.Empty));
        _converter
            .When(x => x.ConvertToText(42))
            .Throw<ArgumentOutOfRangeException>();
        
        var sut = Create();
        
        var response = (ObjectResult) await sut.Get(string.Empty);
        
        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode!.Value);    
    }
    
    [Fact]
    public async Task ShouldReturnInternalServerErrorOnOtherExceptions()
    {
        _parser.Parse(string.Empty)
            .ReturnsForAnyArgs(new CurrencyParseResult(42, true, string.Empty));
        _validator.Validate(42).ReturnsForAnyArgs(new CurrencyValidationResult(CurrencyValidationStatus.Valid, string.Empty));
        _converter
            .When(x => x.ConvertToText(42))
            .Throw<Exception>();
        
        var sut = Create();
        
        var response = (StatusCodeResult) await sut.Get(string.Empty);
        
        Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);    
    }

    private MoneyController Create()
    {
        return new MoneyController(_converter, _validator, _parser);
    }
}