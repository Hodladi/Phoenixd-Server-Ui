namespace Wallet.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.JSInterop;
    using System.Threading.Tasks;

    public interface IConfigurationService
    {
        Task<string> GetBaseUrlAsync();
        Task<string> GetPasswordAsync();
        Task<bool> IsConfiguredInAppSettingsAsync();
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;
        private readonly IJSRuntime _jsRuntime;

        public ConfigurationService(IConfiguration configuration, IJSRuntime jsRuntime)
        {
            _configuration = configuration;
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetBaseUrlAsync()
        {
            var baseUrl = _configuration["AppSettings:BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                return baseUrl;
            }

            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "baseUrl") ?? string.Empty;
        }

        public async Task<string> GetPasswordAsync()
        {
            var password = _configuration["AppSettings:Password"];
            if (!string.IsNullOrEmpty(password))
            {
                return password;
            }

            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "password") ?? string.Empty;
        }

        public async Task<bool> IsConfiguredInAppSettingsAsync()
        {
            var baseUrl = _configuration["AppSettings:BaseUrl"];
            var password = _configuration["AppSettings:Password"];
            return !string.IsNullOrEmpty(baseUrl) && !string.IsNullOrEmpty(password);
        }
    }
}