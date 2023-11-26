using KLA.Domain.Services;
using KLA.Domain.Shared.Services;

namespace KLA.Test.UnitTests.DomainTests;

public class MoneyToTextConverterServiceTests
{
    [Theory]
    [InlineData(0, "zero dollars")]
    [InlineData(1, "one dollar")]
    [InlineData(25.1, "twenty-five dollars and ten cents")]
    [InlineData(0.01, "zero dollars and one cent")]
    [InlineData(45100, "forty-five thousand one hundred dollars")]
    [InlineData(999_999_999.99, "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
    public void ShouldPassSamples(decimal input, string output)
    {
        var sut = Create();
        Assert.Equal(output, sut.ConvertToText(input));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1_000_000_000)]
    public void ShouldCheckRange(decimal input)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Create().ConvertToText(input));
    }

    [Fact]
    public void ShouldRoundToOneCentIfBiggerOrEqualHalfCent()
    {
        var sut = Create();
        Assert.Equal("zero dollars and one cent", sut.ConvertToText(0.005m));
    }
    
    [Fact]
    public void ShouldRoundToZeroIfSmallerThanHalfCent()
    {
        var sut = Create();
        Assert.Equal("zero dollars", sut.ConvertToText(0.0045m));
    }

    [Fact]
    public void ShouldAddPluralIfLastThousandIs0()
    {
        var sut = Create();
        Assert.Equal("one hundred thousand dollars", sut.ConvertToText(100_000m));
    }

    private static ICurrencyToTextConverterService Create()
    {
        return new CurrencyToTextConverterService(new CurrencyRangeValidator());
    }
}