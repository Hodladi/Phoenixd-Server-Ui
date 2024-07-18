namespace Wallet.Models;

public class ApiRequest
{
	public string? Description { get; set; }
	public string? DescriptionHash { get; set; }
	public int? AmountSat { get; set; }
	public int? Amount { get; set; }
	public string? ExternalId { get; set; }
	public string? Invoice { get; set; }
	public string? Offer { get; set; }
	public string? Message { get; set; }
	public string? Address { get; set; }
	public string? Lnurl { get; set; }
	public string? ChannelId { get; set; }
	public int? FeerateSatByte { get; set; }
	public string? Error { get; set; }
}
