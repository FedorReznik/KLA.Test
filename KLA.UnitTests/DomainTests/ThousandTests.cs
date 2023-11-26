using KLA.Domain.Models;

namespace KLA.Test.UnitTests.DomainTests;

public class ThousandTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void ShouldBeEmptyForZeroValueAnyScale(byte scale)
    {
        var sut = new Thousand(0, scale);
        Assert.Equal(string.Empty, sut.ToString());
    }

    [Fact]
    public void ShouldCheckRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Thousand(1000, 0));
    }
    
    [Fact]
    public void ShouldAddNothingForScale0()
    {
        var sut = new Thousand(1, 0);
        Assert.Equal("one", sut.ToString());
    }
    
    [Fact]
    public void ShouldAddThousandForScale1()
    {
        var sut = new Thousand(1, 1);
        Assert.Equal("one thousand", sut.ToString());
    }

    [Fact]
    public void ShouldAddMillionForScale2()
    {
        var sut = new Thousand(1, 2);
        Assert.Equal("one million", sut.ToString());
    }

    [Theory]
    [MemberData(nameof(GetHundreds))]
    public void ShouldAddHundred(ushort input)
    {
        var sut = new Thousand(input, 0);
        Assert.EndsWith(" hundred", sut.ToString());
    }

    public static IEnumerable<object[]> GetHundreds()
    {
        for (int i = 1; i <= 9; i++)
        {
            yield return new object[] { i * 100 };
        }
    }
}