using System;
using System.Net;

namespace MyLookup
{
  class Program
  {
    static void Main(string[] args)
    {
      // 도메인명을 받을 변수를 선언합니다.
      string strHostName = "";

      try
      {
        // 인자로 도메인명이 있는지를 판별합니다.
        if (args.Length > 0)
        {
          // 인자로 주어진 도메인명을 변수에 할당합니다.
          strHostName = args[0];
        }
        else
        {
          // 안내 메시지를 출력하여 도메인명을 입력하도록 합니다.
          Console.WriteLine("도메인명을 입력하세요");
          Console.Write(">");

          // 입력된 도메인명을 변수에 할당합니다.
          strHostName = Console.ReadLine();
        }
        Console.WriteLine();

        // Dns의 메소드를 이용하여 IPHostEntry 객체로 정보를 얻습니다.
        IPHostEntry hostInfo = Dns.GetHostByName(strHostName);

        // 호스트의 실제 도메인 명을 출력합니다. 
        Console.WriteLine("1. Real Name");
        Console.WriteLine(hostInfo.HostName);
        Console.WriteLine();

        // 도메인이 가지고 있는 IP 리스트를 출력합니다.
        Console.WriteLine("2. IP Addresses");
        foreach (IPAddress ip in hostInfo.AddressList)
        {
          Console.WriteLine(ip.ToString());
        }
        Console.WriteLine();

        // 해당 도메인의 별칭 리스트를 출력합니다
        Console.WriteLine("3. Aliases ");
        foreach (string alise in hostInfo.Aliases)
        {
          Console.WriteLine(alise);
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine("Exception caught!!!");
        Console.WriteLine("Source : " + ex.Source);
        Console.WriteLine("Message : " + ex.Message);
      }
    }
  }
}
