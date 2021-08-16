using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

// socket 생성 후 IP주소, 포트번호 얻어온 다음 connect
// 네이버의 첫 페이지를 html로 나타낸 것

namespace SocketEx
{
    class Program
    {
        // 기본적으로 사용할 변수를 선언
        static string _strHostName = "www.naver.com";                                   // == static string _strHostName = "203.241.250.11";
        static string _strPath = "/";                                                   // root

        static void Main(string[] args)
        {
            // 소켓 생성 (TCP 소켓) : C# : AddressFamily.InterNetwork == C : AF_INET
            Socket sckTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   
  
            //DNS 에서 알아본 Resolve() 메소드를 사용하여 Host에 대한 정보를 가져옴
            IPHostEntry ipEntry = Dns.Resolve(_strHostName);                              // IPHostEntry : 도메인 이름을 주면 도메인 IP 정보를 받아옴

            // 아이피 주소 중 첫번째 아이피와 80번 포트를 사용한 종점 생성 
            IPEndPoint ipEndPoint = new IPEndPoint(ipEntry.AddressList[0], 80);           // IPEndPoint : 네트워크 EndPoint를 IP 주소와 포트 번호로 나타냄, ipEntry.AddressList[0] : 2개의 naver 주소 중 처음 주소, 80 : htttp 접속
            IPEndPoint ipEndPoint1 = new IPEndPoint(IPAddress.Parse(textBox?.Text), 80);
            // 127.0.0.1, send receive 호출x
            try
            {
                // Connect() : 소켓을 원격의 종점에 연결을 시도
                sckTcp.Connect(ipEndPoint);

                // Connected : 소켓이 연결되었다면(true) 
                if (sckTcp.Connected)
                {
                    Console.WriteLine("소켓이 연결되었습니다. ");

                    // 소켓을 인자로 Http 프로토콜을 사용한 통신을 구현한 socketSendReceive 메소드를 호출
                    string strContent = socketSendReceive(sckTcp);

                    // 리턴 받은 소스의 내용을 출력
                    Console.WriteLine(strContent);
                }
                else    // 연결되지 않았다면(false)
                    Console.WriteLine("이전에 소켓이 연결되지 않았습니다. ");
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

        // 서버로 요청메소드 Get 메소드 문자열을 생성하여 지정 페이지의 내용을 요청하고 받은 응답 메세지와 페이지의 내용을 보여줌
        private static string socketSendReceive(Socket s)
        {
            // 서버에게 보낼 메소드 문자열을 생성 
            string Get = "GET " + _strPath + " HTTP/1.1\r\n Host: " + _strHostName + "\r\nConnection: Close\r\n\r\n";

            Encoding ASCII = Encoding.ASCII;                  // C# : 유니코드 기반, 유니코드를 아스키 코드 문자열로 변환
            Byte[] ByteGet = ASCII.GetBytes(Get);             // 컴퓨터 내부 : Byte 기반, ASCII 인코딩을 사용하여 Byte 배열로 변환

            // 내용을 받을 바이트 배열을 선언
            Byte[] RecvBytes = new Byte[1024];
            String strRetPage = null;

            if (s == null)
                return ("Connection failed");

            // Send() : 서버에게 요청 명령을 보냄
            s.Send(ByteGet);

            // Receive() : 서버 홈페이지의 내용을 받음
            Int32 bytes = s.Receive(RecvBytes, RecvBytes.Length, SocketFlags.None);

            // 처음 1024 bytes를 읽음
            strRetPage = "Default HTML page on " + _strHostName + ":\r\n";
            strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, bytes);   // ASCII.GetString() : ASCII 인코딩을 사용하여 String 배열로 변환

            // 처음 내용이 있는지 확인한 후 받을 내용이 없을때까지 반복적으로 받음
            while (bytes > 0)
            {
                bytes = s.Receive(RecvBytes, RecvBytes.Length, 0);
                strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, bytes);
            }

            return strRetPage;
        }   
    }
}
