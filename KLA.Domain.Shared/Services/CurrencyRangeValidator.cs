namespace KLA.Domain.Shared.Services;

public class CurrencyRangeValidator : ICurrencyRangeValidator
{
    private const decimal MaxSupportedValue = 999_999_999.99m;
    private const string MessageTemplate = "Only values between 0 and {0} are supported, but got {1}";

    public CurrencyValidationResult Validate(decimal money)
    {
        if (money < 0)
            return new CurrencyValidationResult(CurrencyValidationStatus.TooSmall, string.Format(MessageTemplate, MaxSupportedValue, money));

        if (money > MaxSupportedValue)
            return new CurrencyValidationResult(CurrencyValidationStatus.TooBig, string.Format(MessageTemplate, MaxSupportedValue, money));

        return new CurrencyValidationResult(CurrencyValidationStatus.Valid, string.Empty);
    }
}