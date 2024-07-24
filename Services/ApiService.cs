using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Wallet.Models;
using Microsoft.JSInterop;

namespace Wallet.Services
{
    public interface IApiService
    {
        Task InitializeAsync(string baseUrl, string password);
        Task<ApiResponse?> GetNodeInfoAsync();
        Task<List<OutgoingPayment>?> GetOutgoingPaymentsAsync(int offset = 0);
        Task<List<IncomingPayment>?> GetIncomingPaymentsAsync(int offset = 0);
        Task<string?> LnUrlAuthAsync(string lnurl);
        Task<ApiResponse> GetBolt12OfferAsync();
        Task<string> GetBolt12LnAddress();
        Task<ApiResponse> CreateBolt11InvoiceAsync(ApiRequest apiRequest);
        Task<ApiResponse> PayBolt11Invoice(ApiRequest apiRequest);
        Task<ApiRequest> DecodeBolt11InvoiceAsync(string invoice);
        Task<ApiResponse> PayLnurlInvoice(ApiRequest apiRequest);
        Task<ApiResponse> PayLightningAddress(ApiRequest apiRequest);
        Task<ApiResponse> PayBolt12Invoice(ApiRequest apiRequest);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationService _configurationService;
        private bool _isInitialized;

        public ApiService(HttpClient httpClient, IConfigurationService configurationService)
        {
            _httpClient = httpClient;
            _configurationService = configurationService;
            _isInitialized = false;
        }

        public async Task InitializeAsync(string baseUrl, string password)
        {
            if (_isInitialized) return;

            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Base URL or Password is not set.");
            }

            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{password}")));
            _isInitialized = true;
        }

        public async Task<ApiResponse?> GetNodeInfoAsync()
        {
            await EnsureInitializedAsync();
            var response = await _httpClient.GetAsync("getinfo");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var nodeInfo = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return nodeInfo;
            }

            return null;
        }

        public async Task<List<OutgoingPayment>?> GetOutgoingPaymentsAsync(int offset = 0)
        {
            await EnsureInitializedAsync();
            var response = await _httpClient.GetAsync($"payments/outgoing?offset={offset}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OutgoingPayment>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new List<OutgoingPayment>();
        }

        public async Task<List<IncomingPayment>?> GetIncomingPaymentsAsync(int offset = 0)
        {
            await EnsureInitializedAsync();
            var response = await _httpClient.GetAsync($"payments/incoming?offset={offset}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<IncomingPayment>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new List<IncomingPayment>();
        }

        public async Task<string?> LnUrlAuthAsync(string lnurl)
        {
            await EnsureInitializedAsync();
            var content = new StringContent($"lnurl={lnurl}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _httpClient.PostAsync("lnurlauth", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        public async Task<ApiResponse> PayBolt12Invoice(ApiRequest apiRequest)
        {
            await EnsureInitializedAsync();
            var data = new Dictionary<string, string>
            {
                { "offer", apiRequest.Invoice! },
                { "amountSat", apiRequest.AmountSat!.ToString() },
                { "message", apiRequest.Message! }
            };

            var content = new FormUrlEncodedContent(data);
            var response = new ApiResponse();
            var httpResponse = await _httpClient.PostAsync("payoffer", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return response;
            }
            else
            {
                response.Error = "Could not pay invoice";
                return response;
            }
        }

        public async Task<ApiResponse> PayLightningAddress(ApiRequest apiRequest)
        {
            await EnsureInitializedAsync();
            var data = new Dictionary<string, string>
            {
                { "address", apiRequest.Invoice },
                { "amountSat", apiRequest.AmountSat.ToString() },
                { "message", apiRequest.Message }
            };

            var content = new FormUrlEncodedContent(data);
            var response = new ApiResponse();
            var httpResponse = await _httpClient.PostAsync("paylnaddress", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return response;
            }
            else
            {
                response.Error = "Could not pay invoice";
                return response;
            }
        }

        public async Task<ApiResponse> PayLnurlInvoice(ApiRequest apiRequest)
        {
            await EnsureInitializedAsync();
            var data = new Dictionary<string, string>
            {
                { "lnurl", apiRequest.Invoice },
                { "amountSat", apiRequest.AmountSat.ToString() },
                { "message", apiRequest.Message }
            };

            var content = new FormUrlEncodedContent(data);
            var response = new ApiResponse();
            var httpResponse = await _httpClient.PostAsync("lnurlpay", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return response;
            }
            else
            {
                response.Error = "Could not pay invoice";
                return response;
            }
        }

        public async Task<ApiResponse> PayBolt11Invoice(ApiRequest apiRequest)
        {
            await EnsureInitializedAsync();
            var data = new Dictionary<string, string>
            {
                { "invoice", apiRequest.Invoice }
            };

            if (apiRequest.AmountSat.HasValue)
            {
                data.Add("amountSat", apiRequest.AmountSat.Value.ToString());
            }

            var content = new FormUrlEncodedContent(data);
            var response = new ApiResponse();
            var httpResponse = await _httpClient.PostAsync("payinvoice", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                response.AmountSat = apiRequest.AmountSat;
                response.Message = apiRequest.Description;
                return response;
            }
            else
            {
                response.Error = "Could not pay invoice";
                return response;
            }
        }

        public async Task<ApiRequest> DecodeBolt11InvoiceAsync(string invoice)
        {
            await EnsureInitializedAsync();
            var content = new StringContent($"invoice={invoice}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = new ApiRequest();
            var httpResponse = await _httpClient.PostAsync("decodeinvoice", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<ApiRequest>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                response!.Invoice = invoice;
                response!.AmountSat = response.Amount / 1000;
                return response;
            }
            else
            {
                response.Error = "Could not get invoice";
                return response;
            }
        }

        public async Task<ApiResponse> CreateBolt11InvoiceAsync(ApiRequest apiRequest)
        {
            await EnsureInitializedAsync();
            var response = new ApiResponse();
            var content = new StringContent($"description={apiRequest.Description}&amountSat={apiRequest.AmountSat}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var httpResponse = await _httpClient.PostAsync("createinvoice", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                response.InvoiceString = responseContent;
                response = JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return response;
            }
            else
            {
                response.Error = "Could not create invoice";
                return response;
            }
        }

        public async Task<ApiResponse> GetBolt12OfferAsync()
        {
            await EnsureInitializedAsync();
            var response = new ApiResponse();
            var httpResponse = await _httpClient.GetAsync("getoffer");

            if (httpResponse.IsSuccessStatusCode)
            {
                response.Offer = await httpResponse.Content.ReadAsStringAsync();
                return response;
            }
            else
            {
                response.Error = "Could not fetch Bolt12 Offer";
                return response;
            }
        }

        public async Task<string> GetBolt12LnAddress()
        {
            await EnsureInitializedAsync();
            var response = await _httpClient.GetAsync("getlnaddress");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Could not get Bolt 12 LN address";
            }
        }

        private async Task EnsureInitializedAsync()
        {
            if (!_isInitialized)
            {
                var baseUrl = await _configurationService.GetBaseUrlAsync();
                var password = await _configurationService.GetPasswordAsync();
                await InitializeAsync(baseUrl, password);
            }
        }
    }
}
