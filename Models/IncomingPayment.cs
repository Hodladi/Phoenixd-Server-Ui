namespace Wallet.Models;

public class IncomingPayment
{
	public string PaymentHash { get; set; }
	public string Preimage { get; set; }
	public string ExternalId { get; set; }
	public string Description { get; set; }
	public string Invoice { get; set; }
	public bool IsPaid { get; set; }
	public long ReceivedSat { get; set; }
	public long Fees { get; set; }
	public long? CompletedAt { get; set; }
	public long CreatedAt { get; set; }
}
