using System.Net.WebSockets;
using System.Text;
using Websocket.Client;

namespace Wallet.Services
{
    public interface IWebSocketService
    {
        event Action<string> OnMessageReceived;
        Task ConnectAsync(string baseUrl, string authToken);
        Task DisconnectAsync();
    }

    public class WebSocketService : IWebSocketService
    {
        private WebsocketClient _client;

        public event Action<string> OnMessageReceived;

        public async Task ConnectAsync(string baseUrl, string authToken)
        {
            if (_client != null && _client.IsRunning)
            {
                return; // Already connected
            }

            var wsUri = new Uri(baseUrl.Replace("http://", "ws://").Replace("https://", "wss://"));

            var url = wsUri;
            var factory = new Func<ClientWebSocket>(() =>
            {
                var client = new ClientWebSocket();
                var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(":" + authToken));
                client.Options.SetRequestHeader("Authorization", $"Basic {token}");
                return client;
            });

            _client = new WebsocketClient(url, factory)
            {
                ReconnectTimeout = TimeSpan.FromSeconds(30)
            };

            _client.ReconnectionHappened.Subscribe(info => Console.WriteLine($"Reconnection happened, type: {info.Type}"));
            _client.MessageReceived.Subscribe(msg => OnMessageReceived?.Invoke(msg.Text));

            await _client.Start();
        }

        public Task DisconnectAsync()
        {
            _client.Dispose();
            return Task.CompletedTask;
        }
    }
}