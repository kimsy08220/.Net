using System;
using System.Net;

namespace IPAddressEx
{
  class Program
  {
    static void Main(string[] args)
    {
      // 1. 읽기전용 값들을 나열 합니다.
      Console.WriteLine("IPAddress 의 읽기전용 값들을 나열 합니다.");
      Console.WriteLine("IPAddress.Any : {0}", IPAddress.Any.ToString());
      Console.WriteLine("IPAddress.Broadcast : {0}", IPAddress.Broadcast.ToString());
      Console.WriteLine("IPAddress.Loopback : {0}", IPAddress.Loopback.ToString());
      Console.WriteLine("IPAddress.None : {0}", IPAddress.None.ToString());
      Console.WriteLine();

      // 2. IPAddress 인스턴스 생성하기 
      IPAddress ipAddress = IPAddress.Parse("211.233.62.105");

      // 3. 어떤 주소체계를 가지고 있는지 나타냅니다.
      Console.WriteLine("AddressFamily : {0}", ipAddress.AddressFamily.ToString());

      // 4. Loopback 인지를 체크합니다.
      if (IPAddress.IsLoopback(ipAddress))
        Console.WriteLine("{0} is Loopback.", ipAddress.ToString());
      else
        Console.WriteLine("{0} is not Loopback", ipAddress.ToString());
    }
  }
}
