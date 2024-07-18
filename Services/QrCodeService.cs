using System.Text;
using Net.Codecrete.QrCodeGenerator;

namespace Wallet.Services;

public interface IQrCodeService
{
    Task<string?> GenerateQrCode(string text);
}

public class QrCodeService : IQrCodeService
{
    public Task<string?> GenerateQrCode(string text)
    {
        var qr = QrCode.EncodeText(text, QrCode.Ecc.Low);
        byte[] svgBytes = Encoding.UTF8.GetBytes(qr.ToSvgString(1));
        return Task.FromResult(Convert.ToBase64String(svgBytes));
    }
}
