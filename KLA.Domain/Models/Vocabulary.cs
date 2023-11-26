namespace KLA.Domain.Models;

public static class Vocabulary
{
    public const string HundredText = "hundred";
    public const string DollarText = "dollar";
    public const string CentText = "cent";
    public const char PluralText = 's';
    public const string ZeroDollarsText = "zero dollars";
    
    public static readonly IReadOnlyList<string> OneToTwentyNumbersTexts = new [] 
    {
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
        "ten",
        "eleven",
        "twelve",
        "thirteen",
        "fourteen",
        "fifteen",
        "sixteen",
        "seventeen",
        "eighteen",
        "nineteen"
    };

    public static readonly IReadOnlyList<string> TenthsTexts = new [] 
    {
        "ten",
        "twenty",
        "thirty",
        "forty",
        "fifty",
        "sixty",
        "seventy",
        "eighty",
        "ninety"
    };

    public static readonly IReadOnlyList<string> ThousandsTexts = new [] 
    {
        "thousand",
        "million"
    };
}