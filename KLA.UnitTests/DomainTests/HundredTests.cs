using KLA.Domain.Models;

namespace KLA.Test.UnitTests.DomainTests;

public class HundredTests
{
    [Fact]
    public void ShouldBeEmptyForZeroValue()
    {
        var sut = new Hundred(0);
        Assert.Equal(string.Empty, sut.ToString());
    }
    
    [Fact]
    public void ShouldCheckRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Hundred(100));
    }

    [Fact]
    public void ShouldHandleTenthsWithoutDigits()
    {
        var tenth = 3;
        var sut = new Hundred((byte) (tenth * 10));
        Assert.Equal(Vocabulary.TenthsTexts[tenth - 1], sut.ToString());
    }
    
    [Fact]
    public void ShouldHandleTenthsWithDigits()
    {
        var tenth = 3;
        var digit = 2;
        var sut = new Hundred((byte) (tenth * 10 + 2));
        Assert.Equal($"{Vocabulary.TenthsTexts[tenth - 1]}-{Vocabulary.OneToTwentyNumbersTexts[digit - 1]}", sut.ToString());
    }

    [Theory]
    [MemberData(nameof(GetValuesLessThan20))]
    public void ShouldHandleNumbersLessThan20(byte input, string expected)
    {
        var sut = new Hundred(input);
        Assert.Equal(expected, sut.ToString());
    }
    
    public static IEnumerable<object[]> GetValuesLessThan20()
    {
        for (byte i = 1; i < 20; i++)
        {
            yield return new object[] {i, Vocabulary.OneToTwentyNumbersTexts[i - 1]};
        }
    }
}