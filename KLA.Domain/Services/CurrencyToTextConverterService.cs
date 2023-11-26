using System.Text;
using KLA.Domain.Models;
using KLA.Domain.Shared.Services;

namespace KLA.Domain.Services;

public class CurrencyToTextConverterService : ICurrencyToTextConverterService
{
    private const int OneThousandScale = 1000;
    
    private readonly ICurrencyRangeValidator _currencyRangeValidator;

    public CurrencyToTextConverterService(ICurrencyRangeValidator currencyRangeValidator)
    {
        _currencyRangeValidator = currencyRangeValidator ?? throw new ArgumentNullException(nameof(currencyRangeValidator));
    }

    public string ConvertToText(decimal money)
    {
        var validationStatus = _currencyRangeValidator.Validate(money);
        if (validationStatus.Status != CurrencyValidationStatus.Valid)
            throw new ArgumentOutOfRangeException(nameof(money), money, validationStatus.Message);

        var roundedMoney = Math.Round(money, 2, MidpointRounding.AwayFromZero);

        if (roundedMoney == 0)
        {
            return Vocabulary.ZeroDollarsText;
        }

        var integerPart = (uint) roundedMoney;
        var fractionalPart = (byte) ((roundedMoney - integerPart) * 100);

        var integerPartThousands = SplitByThousand(integerPart);
        var fractionHundred = new Hundred(fractionalPart);

        var text = BuildText(integerPartThousands, fractionHundred);

        return text;
    }
    
    private static IList<Thousand> SplitByThousand(uint number)
    {
        var thousandScale = (byte) Math.Log(number, OneThousandScale);
        
        var result = new List<Thousand>();
        
        for (var i = thousandScale; i > 0 ; i--)
        {
            var currentScale = (uint) Math.Pow(OneThousandScale, i);
            var belowThousand = (ushort) (number / currentScale);
            result.Add(new Thousand(belowThousand, i));
            
            number -= belowThousand * currentScale;
        }
        
        result.Add(new Thousand((ushort)number, 0));

        return result;
    }

    private static string BuildText(IList<Thousand> integerPartThousandsReversed, Hundred fractionHundred)
    {
        var builder = new StringBuilder();

        var nextNotZeroShouldAddWhitespace = false;
        foreach (var thousand in integerPartThousandsReversed)
        {
            if (nextNotZeroShouldAddWhitespace && thousand.Actual > 0)
            {
                // adding white-space between non-empty thousands
                builder.Append(' ');
            }

            var currentBuilderLength = builder.Length;
            thousand.AppendToText(builder);
            nextNotZeroShouldAddWhitespace = builder.Length > currentBuilderLength;
        }

        if (builder.Length == 0)
        {
            builder.Append(Vocabulary.ZeroDollarsText);
        }
        else 
        {
            builder.Append(' ');
            builder.Append(Vocabulary.DollarText);
            if (integerPartThousandsReversed.Count > 1 // number is bigger than 1000 -> definitely plural 
                || (integerPartThousandsReversed.Count == 1 && integerPartThousandsReversed[0].Actual > 1) // number is less than 1000, but bigger than 1 -> plural
                )
            {
                builder.Append(Vocabulary.PluralText);
            }
        }

        if (fractionHundred.Actual > 0)
        {
            builder.Append(" and ");
            fractionHundred.AppendToText(builder);
            builder.Append(' ');
            builder.Append(Vocabulary.CentText);
            if (fractionHundred.Actual > 1)
            {
                builder.Append(Vocabulary.PluralText);
            }
        }

        return builder.ToString();
    }
}