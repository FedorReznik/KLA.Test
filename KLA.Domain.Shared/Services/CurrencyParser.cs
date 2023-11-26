using System.Globalization;

namespace KLA.Domain.Shared.Services;

public class CurrencyParser : ICurrencyParser
{
    private readonly NumberFormatInfo _currencyFormat = new()
    {
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = " "
    };
    public CurrencyParseResult Parse(string money)
    {
        var parsed = decimal.TryParse(
            money,
            NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint,
            _currencyFormat,
            out var result);
        
        return new CurrencyParseResult(
            result,
            parsed,
            parsed ? string.Empty : $"Please specify comma separated currency value, got: {money} instead");
    }
}