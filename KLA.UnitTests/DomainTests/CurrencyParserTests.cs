using KLA.Domain.Shared.Services;

namespace KLA.Test.UnitTests.DomainTests;

public class CurrencyParserTests
{
    [Fact]
    public void ShouldNotParseDot()
    {
        var sut = new CurrencyParser();
        Assert.False(sut.Parse("1.1").Parsed);
    }
    
    [Fact]
    public void ShouldNotParseCharacters()
    {
        var sut = new CurrencyParser();
        Assert.False(sut.Parse("$1").Parsed);
    }
    
    [Fact]
    public void ShouldParseComma()
    {
        var sut = new CurrencyParser();
        Assert.True(sut.Parse("1,1").Parsed);
    }
    
    [Fact]
    public void ShouldParseSpaceDelimited()
    {
        var sut = new CurrencyParser();
        Assert.True(sut.Parse("42 000,1").Parsed);
    }
    
    [Fact]
    public void ShouldParseNonDelimited()
    {
        var sut = new CurrencyParser();
        Assert.True(sut.Parse("42000,1").Parsed);
    }
    
    [Fact]
    public void ShouldParseWithoutFractionPart()
    {
        var sut = new CurrencyParser();
        Assert.True(sut.Parse("42").Parsed);
    }
}