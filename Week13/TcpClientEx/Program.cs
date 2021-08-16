using System;
using System.Text;
using System.Net.Sockets;

namespace TcpClientEx
{
  class Program
  {
    // 기본 사이트를 microsoft 로 지정합니다.
    static string _strHostName = "www.microsoft.com";
    // 가져온ㄹ 
    static string _strPath = "/";

    static void Main(string[] args)
    {

      // 소켓 생성 
      // Socket sckTcp = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
      // IPHostEntry ipEntry = Dns.Resolve(_strHostName);
      // IPEndPoint ipEndPoint = new IPEndPoint(ipEntry.AddressList[0], 80);
      TcpClient tclient = new TcpClient(_strHostName, 80);

      try
      {
        // TcpClient 를 생성하면서 연결되었으므로 연결부분이 필요가 없어졌습니다.
        //sckTcp.Connect( ipEndPoint );

        // 소켓이 연결되었다면  
        if (tclient.Connected)
        { 
          Console.WriteLine("소켓이 연결되었습니다. ");
          string strContent = socketSendReceive(tclient);
          Console.WriteLine(strContent);
        }
        else
        {
          Console.WriteLine("이전에 소켓이 연결되지 않았습니다. ");
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine("Exception Thrown: " + ex.ToString());
      }
      finally
      {
        tclient.Close();
      }

    }

    // 서버로 요청메소드 Get 메소드 문자열을 생성하여 지정 페이지의 내용을 요청하고
    // 받은 응답 메세지와 페이지의 내용을 보여준다. 
    private static string socketSendReceive(TcpClient s)
    {
      NetworkStream ns = s.GetStream();

      // 서버에게 보낼 메소드 문자열을 생성합니다. 
      Encoding ASCII = Encoding.ASCII;
      string Get = "GET " + _strPath + " HTTP/1.1\r\n Host: " + _strHostName + "\r\nConnection: Close\r\n\r\n";
      Byte[] ByteGet = ASCII.GetBytes(Get);

      Byte[] RecvBytes = new Byte[1024];
      String strRetPage = null;

      if (s == null)
        return ("Connection failed");

      // 서버에게 요청 명령을 보냅니다..
      //s.Send(ByteGet);  
      ns.Write(ByteGet, 0, ByteGet.Length);


      // 서버 홈페이지의 내용을 받습니다.
      // Int32 bytes = s.Receive(RecvBytes, RecvBytes.Length, SocketFlags.None);
      int bytes = ns.Read(RecvBytes, 0, RecvBytes.Length);

      // 처음 1024 bytes 를 읽습니다.
      strRetPage = "Default HTML page on " + _strHostName + ":\r\n";
      strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, bytes);

      while (bytes > 0)
      {
        //bytes = s.Receive(RecvBytes, RecvBytes.Length, 0);
        bytes = ns.Read(RecvBytes, 0, RecvBytes.Length);
        strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, bytes);
      }

      return strRetPage;
    }

  }
}
