namespace Wallet.Models;

public class ApiResponse
{
	public string? LnUrlAuth { get; set; }
    public string? InvoiceString { get; set; }
	public int? AmountSat { get; set; }
	public string? PaymentHash { get; set; }
	public string? Serialized { get; set; }
	public int? RecipientAmountSat { get; set; }
	public int? RoutingFeeSat { get; set; }
	public string? PaymentId { get; set; }
	public string? PaymentPreimage { get; set; }
	public string? Offer { get; set; }
	public string? NodeId { get; set; }
	public string? Address { get; set; }
	public string? TxId { get; set; }
	public List<Channel>? Channels { get; set; }
	public List<Payment>? Payments { get; set; }
	public string? Message { get; set; }
	public string? ExternalId { get; set; }
	public string? Preimage { get; set; }
	public bool? IsPaid { get; set; }
	public int? ReceivedSat { get; set; }
	public int? Fees { get; set; }
	public long? CompletedAt { get; set; }
	public long? CreatedAt { get; set; }
	public string? Type { get; set; }
	public string? Error { get; set; }
}

public class Channel
{
	public string State { get; set; }
	public string ChannelId { get; set; }
	public int BalanceSat { get; set; }
	public int InboundLiquiditySat { get; set; }
	public int CapacitySat { get; set; }
	public string FundingTxId { get; set; }
}

public class Payment
{
	public string PaymentId { get; set; }
	public string PaymentHash { get; set; }
	public string Preimage { get; set; }
	public bool IsPaid { get; set; }
	public int Sent { get; set; }
	public int Fees { get; set; }
	public string Invoice { get; set; }
	public long CompletedAt { get; set; }
	public long CreatedAt { get; set; }
}
