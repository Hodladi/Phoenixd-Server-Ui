using Wallet.Enums;

namespace Wallet.Models;

public class Transaction
{
    public DateTime Date { get; set; }
    public long Amount { get; set; }
    public TransactionType Type { get; set; }
    public long Fee { get; set; }
    public object OriginalTransaction { get; set; }
}
