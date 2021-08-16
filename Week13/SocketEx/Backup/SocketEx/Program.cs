using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace SocketEx
{
  class Program
  {
	// 기본적으로 사용할 변수를 선언합니다
    static string _strHostName = "www.microsoft.com";
    static string _strPath = "/";

    static void Main(string[] args)
    {

      // 소켓 생성 
      Socket sckTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      //Dns 에서 알아본 Resolve()메소드를 사용하여 Host에 대한 정보를 가져옵니다
	  IPHostEntry ipEntry = Dns.Resolve(_strHostName);
      // 아이피 주소중 첫번째 아이피와 80번 포트를 사용한 종점 생성
	  IPEndPoint ipEndPoint = new IPEndPoint(ipEntry.AddressList[0], 80);

      try
      {
		// 소켓을 원격의 종점에 연결을 시도합니다.
        sckTcp.Connect(ipEndPoint);
		
		// 소켓이 연결되었다면 
        if (sckTcp.Connected)
        { 
          Console.WriteLine("소켓이 연결되었습니다. ");
          
		  // 소켓을 인자로 Http 프로토콜을 사용한 통신을 구현한 
		  // socketSendReceive 메소드를 호출합니다.
		  string strContent = socketSendReceive(sckTcp);
          
		  // 리턴 받은 소스의 내용을 출력합니다.
		  Console.WriteLine(strContent);
        }
        else
        {
		  // 연결되지 않았다면
          Console.WriteLine("이전에 소켓이 연결되지 않았습니다. ");
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine("Exception Thrown: " + ex.ToString());
      }
      finally
      {
        sckTcp.Close();
      }
    }

    // 서버로 요청메소드 Get 메소드 문자열을 생성하여 지정 페이지의 내용을 요청하고
    // 받은 응답 메세지와 페이지의 내용을 보여준다. 
    private static string socketSendReceive(Socket s)
    {
      // 서버에게 보낼 메소드 문자열을 생성합니다. 
      string Get = "GET " + _strPath + " HTTP/1.1\r\n Host: " + _strHostName + "\r\nConnection: Close\r\n\r\n";
      
	  // ASCII 인코딩을 사용하여 Byte 배열로 변환 합니다.
	  Encoding ASCII = Encoding.ASCII;
	  Byte[] ByteGet = ASCII.GetBytes(Get);

	  // 내용을 받을 바이트 배열을 선언합니다.
      Byte[] RecvBytes = new Byte[1024];
      String strRetPage = null;

      if (s == null)
        return ("Connection failed");

      // 서버에게 요청 명령을 보냅니다..
      s.Send(ByteGet);

      // 서버 홈페이지의 내용을 받습니다.
      Int32 bytes = s.Receive(RecvBytes, RecvBytes.Length, SocketFlags.None);

      // 처음 1024 bytes 를 읽습니다.
      strRetPage = "Default HTML page on " + _strHostName + ":\r\n";
      strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, bytes);

	  // 처음 내용이 있는지 확인 한 후 받을 내용이 없을때까지 반복적으로 받음
      while (bytes > 0)
      {
        bytes = s.Receive(RecvBytes, RecvBytes.Length, 0);
        strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, bytes);
      }

      return strRetPage;
    }// end socketSendReceive
  }// end Class 
}// end Namespace
