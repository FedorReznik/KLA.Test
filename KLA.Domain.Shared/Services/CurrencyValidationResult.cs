namespace KLA.Domain.Shared.Services;

public record CurrencyValidationResult
{
    public CurrencyValidationResult(CurrencyValidationStatus status, string message)
    {
        Status = status;
        Message = message;
    }

    public CurrencyValidationStatus Status { get; }
    public string Message { get; } 
}