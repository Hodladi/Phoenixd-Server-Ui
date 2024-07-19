using Blazored.Modal;
using Wallet.Components;
using Wallet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(System.Net.IPAddress.Any, 2291);
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient<IApiService, ApiService>();
builder.Services.AddScoped<IInvoiceTypeService, InvoiceTypeService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();
builder.Services.AddSingleton<IWebSocketService, WebSocketService>();
builder.Services.AddBlazoredModal();

builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromMinutes(60);
        options.HandshakeTimeout = TimeSpan.FromMinutes(60);
        options.KeepAliveInterval = TimeSpan.FromMinutes(10);
    })
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromHours(1);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2),
    ReceiveBufferSize = 4 * 1024
};

app.UseWebSockets(webSocketOptions);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();