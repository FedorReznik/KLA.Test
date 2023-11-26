namespace KLA.Domain.Shared.Services;

public interface ICurrencyParser
{
    CurrencyParseResult Parse(string money);
}