namespace KLA.Domain.Shared.Services;

public record CurrencyParseResult
{
    public CurrencyParseResult(decimal? parsedValue, bool parsed, string message)
    {
        ParsedValue = parsedValue;
        Parsed = parsed;
        Message = message;
    }

    public decimal? ParsedValue { get; }
    public bool Parsed { get; }
    public string Message { get; }
}