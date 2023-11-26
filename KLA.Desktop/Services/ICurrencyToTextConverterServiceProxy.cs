using System.Threading.Tasks;

namespace KLA.Desktop.Services;

public interface ICurrencyToTextConverterServiceProxy
{
    Task<string> Convert(string money);
}