namespace Wallet.Models;

public class PaymentNotification
{
    public string Type { get; set; }
    public long? Timestamp { get; set; }
    public long AmountSat { get; set; }
    public string PaymentHash { get; set; }
    public string? ExternalId { get; set; }
}
