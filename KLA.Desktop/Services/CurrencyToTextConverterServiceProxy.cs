using System;
using System.Net;
using System.Threading.Tasks;
using KLA.Desktop.Exceptions;
using KLA.Desktop.Models;
using RestSharp;

namespace KLA.Desktop.Services;

public class CurrencyToTextConverterServiceProxy : ICurrencyToTextConverterServiceProxy, IDisposable
{
    private readonly RestClient _restClient;

    public CurrencyToTextConverterServiceProxy(Settings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        var options = new RestClientOptions(settings.ServerAddress);
        
        _restClient = new RestClient(options);
    }

    public async Task<string> Convert(string money)
    {
        var request = new RestRequest("api/currency/text/{money}").AddUrlSegment("money", money);
        
        var response = await _restClient.ExecuteGetAsync<string>(request);
        
        switch (response.StatusCode)
        {
            case 0:
                throw new ServerException("Server unavailable");
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.InternalServerError:
                throw new ServerException(response.ErrorMessage);
            case HttpStatusCode.NotFound:
                throw new ServerException($"Endpoint: {request.Resource} is not found");
            case HttpStatusCode.OK:
                return response.Data!;
            default:
                throw new ServerException($"Unknown server error: {response.ErrorMessage}");
        }
    }

    public void Dispose()
    {
        _restClient.Dispose();
    }
}