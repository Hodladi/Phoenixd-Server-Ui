namespace Wallet.Enums;

public enum InvoiceType
{
    Bolt11Invoice,
    Bolt12Offer,
    LightningAddress,
    OnChainAddress,
    LNURLPay,
    LNURLAuth,
    Unknown
}
