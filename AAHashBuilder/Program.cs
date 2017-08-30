using System;
using System.Security.Cryptography;
using System.Text;

namespace AAHashBuilder
{
  public class Program
  {
    public static void Main(string[] args)
    {
      if (args.Length == 0 || args.Length == 1)
      {
        Console.WriteLine("Please enter EndPoint ID & Secret");
        Console.WriteLine(@"Usage: 'endpoint id' 'secret'");
      }
      else
      {
        string endpointid = Convert.ToString(args[0].Trim());
        string secret = Convert.ToString(args[1].Trim());
        string salt = CreateSalt();
        Console.WriteLine("salt:        " + salt);
        Console.WriteLine("result hash: " + CreateSecretHash(endpointid, salt, secret));
      }
      Console.ReadKey();
    }

    public static string CreateSalt()
    {
      var size = 32;
      //Generate a cryptographic random number.
      var rng = RandomNumberGenerator.Create();
      var buff = new byte[size];
      rng.GetBytes(buff);

      // Return a Base64 string representation of the random number.
      return Convert.ToBase64String(buff);
    }

    public static string CreateSecretHash(string endpointId, string salt, string secret)
    {
      var middleresult = Sha256HashString(string.Concat(endpointId, salt));
      return Sha256HashString(string.Concat(secret, middleresult));
    }

    public static string Sha256HashString(string s)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(s);

      var sha = SHA256.Create();
      byte[] hashBytes = sha.ComputeHash(bytes);

      return HexString(hashBytes);
    }

    public static string HexString(byte[] bytes)
    {
      var sb = new StringBuilder();
      foreach (byte b in bytes)
      {
        var hex = b.ToString("x2");
        sb.Append(hex);
      }
      return sb.ToString();
    }
  }
}
