using System.Text;

namespace Wallet.Services;

public static class Bech32
{
	private const string Bech32Chars = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";

	public static string Decode(string bech32)
	{
		var data = new byte[bech32.Length - 1];
		for (int i = 0; i < bech32.Length; i++)
		{
			data[i] = (byte)Bech32Chars.IndexOf(bech32[i]);
		}

		var hrp = bech32.Substring(0, bech32.LastIndexOf('1'));
		var decodedData = new byte[hrp.Length + data.Length];
		Buffer.BlockCopy(Encoding.ASCII.GetBytes(hrp), 0, decodedData, 0, hrp.Length);
		Buffer.BlockCopy(data, 0, decodedData, hrp.Length, data.Length);

		var decodedString = Encoding.UTF8.GetString(decodedData);
		return decodedString.Substring(hrp.Length + 1);
	}
}
