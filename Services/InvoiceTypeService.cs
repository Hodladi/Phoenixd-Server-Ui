using global::Wallet.Enums;
using System.Text.RegularExpressions;
using Wallet.Models;

namespace Wallet.Services;

public interface IInvoiceTypeService
{
    InvoiceType DetermineInvoiceType(string qrContent);
}

public class InvoiceTypeService : IInvoiceTypeService
{
    public InvoiceType DetermineInvoiceType(string qrContent)
    {
        if (qrContent.StartsWith("lnbc") || qrContent.StartsWith("lntb"))
            return InvoiceType.Bolt11Invoice;
        if (qrContent.Contains("lno"))
            return InvoiceType.Bolt12Offer;
        if (qrContent.Contains("@"))
            return InvoiceType.LightningAddress;
        if (Regex.IsMatch(qrContent, @"^[13][a-km-zA-HJ-NP-Z1-9]{25,34}$") || Regex.IsMatch(qrContent, @"^bc1[ac-hj-np-z02-9]{11,71}$"))
            return InvoiceType.OnChainAddress;
        if (qrContent.Contains("lnurl1"))
            return InvoiceType.LNURLPay;
        if (qrContent.Contains("lnurla"))
            return InvoiceType.LNURLAuth;

        return InvoiceType.Unknown;
    }
}

