using KLA.Domain.Shared.Services;

namespace KLA.Test.UnitTests.DomainTests;

public class CurrencyRangeValidatorTests
{
    [Fact]
    public void ShouldReturnTooSmallForLessThan0()
    {
        var sut = new CurrencyRangeValidator();
        Assert.Equal(CurrencyValidationStatus.TooSmall, sut.Validate(-1).Status);
    }
    
    [Fact]
    public void ShouldReturnTooSmallForBillionPlus()
    {
        var sut = new CurrencyRangeValidator();
        Assert.Equal(CurrencyValidationStatus.TooBig, sut.Validate(1_000_000_000).Status);
    }
    
    [Fact]
    public void ShouldReturnValidInBetweenZeroAndBillion()
    {
        var sut = new CurrencyRangeValidator();
        Assert.Equal(CurrencyValidationStatus.Valid, sut.Validate(42).Status);
    }
}