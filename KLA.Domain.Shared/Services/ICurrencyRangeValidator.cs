namespace KLA.Domain.Shared.Services;

public interface ICurrencyRangeValidator
{
    CurrencyValidationResult Validate(decimal money);
}