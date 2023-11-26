namespace KLA.Domain.Services;

public interface ICurrencyToTextConverterService
{
    string ConvertToText(decimal money);
}