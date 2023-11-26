using KLA.Desktop.Exceptions;
using KLA.Desktop.Services;
using KLA.Desktop.ViewModels;
using KLA.Domain.Shared.Services;
using NSubstitute;

namespace KLA.Test.UnitTests.ViewModelTests;

public class MainWindowViewModelTests
{
    private readonly ICurrencyParser _currencyParser = Substitute.For<ICurrencyParser>();
    private readonly ICurrencyRangeValidator _rangeValidator = Substitute.For<ICurrencyRangeValidator>();
    private readonly ICurrencyToTextConverterServiceProxy _currencyToTextConverterService = Substitute.For<ICurrencyToTextConverterServiceProxy>();
    
    [Theory]
    [InlineData(CurrencyValidationStatus.TooSmall)]
    [InlineData(CurrencyValidationStatus.TooBig)]
    public void ShouldReactOnValidationErrors(CurrencyValidationStatus status)
    {
        _currencyParser.Parse(string.Empty).ReturnsForAnyArgs(new CurrencyParseResult(42, true, string.Empty));
        _rangeValidator.Validate(42).ReturnsForAnyArgs(new CurrencyValidationResult(status, string.Empty));
        
        var sut = Create();
        sut.CurrencyInput = "42";
        
        Assert.True(sut.HasErrors);
    }

    [Fact]
    private void ShouldReactOnParsingErrors()
    {
        _currencyParser.Parse(string.Empty).ReturnsForAnyArgs(new CurrencyParseResult(42, false, string.Empty));
        
        var sut = Create();
        sut.CurrencyInput = "42";
        
        Assert.True(sut.HasErrors);
    }

    [Fact]
    private void ShouldReactOnServerErrors()
    {
        _currencyParser.Parse(string.Empty).ReturnsForAnyArgs(new CurrencyParseResult(42, true, string.Empty));
        _rangeValidator.Validate(42).ReturnsForAnyArgs(new CurrencyValidationResult(CurrencyValidationStatus.Valid, string.Empty));
        _currencyToTextConverterService
            .When(x => x.Convert("42"))
            .Throw(new ServerException("exception"));
        
        var sut = Create();
        sut.CurrencyInput = "42";
        sut.ConvertCommand.Execute(null);
        
        Assert.True(sut.HasServerError);
    }

    [Fact]
    private void ShouldShowConversionResult()
    {
        var convertedResult = "forty-two";
        _currencyParser.Parse(string.Empty).ReturnsForAnyArgs(new CurrencyParseResult(42, true, string.Empty));
        _rangeValidator.Validate(42).ReturnsForAnyArgs(new CurrencyValidationResult(CurrencyValidationStatus.Valid, string.Empty));
        _currencyToTextConverterService.Convert("42").Returns(Task.FromResult(convertedResult));
        
        var sut = Create();
        sut.CurrencyInput = "42";
        sut.ConvertCommand.Execute(null);
        
        Assert.Equal(convertedResult, sut.CurrencyText);
    }

    private IMainWindowViewModel Create()
    {
        return new MainWindowViewModel(_currencyParser, _rangeValidator, _currencyToTextConverterService);
    }
}