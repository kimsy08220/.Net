using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;               // TcpListener를 사용하기 위해 선언

namespace _190529_TcpServerEx
{
    class Program
    {
        static byte[] recevbyte = new byte[513];

        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 9000);            // TcpListener : TCP 네트워크 클라이언트(포트번호 : 9000)에서 연결을 수신
            listener.Start();

            while (true)
            {
                Encoding ASCII = Encoding.ASCII;
                TcpClient client = listener.AcceptTcpClient();                      // AcceptTcpClient : client 접속
                Socket s = client.Client;                                           // Client 소켓 생성
                
                string address = s.RemoteEndPoint.ToString();                       // socket의 원격 EndPoint 정보를 조사 ex) 127.0.0.1:51877
                string[] array = address.Split(new char[] { ':' });                 // address를 ':'으로 자름, array[0] : 127.0.0.1(IP 주소), array[1] : 51877(포트 번호)
                Console.WriteLine("[TCP 서버] 클라이언트 접속 : IP 주소 = {0}, 포트번호 = {1}", array[0], array[1]);

                try
                {
                    if (client.Connected)
                    {
                        NetworkStream ns = client.GetStream();
                        while (true)
                        {
                            int bytes = ns.Read(recevbyte, 0, recevbyte.Length);    // Read() : client의 정보를 얻어옴
                            String strbyte = ASCII.GetString(recevbyte, 0, bytes);
                            Console.WriteLine("[TCP/{0}:{1}] {2}", array[0], array[1], strbyte);
                            ns.Write(recevbyte, 0, bytes);                          // Write() : client의 정보를 화면에 출력
                        }   
                    }
                    else
                        Console.WriteLine("이전에 소켓이 연결되지 않았습니다.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception Thrown: " + ex.ToString());
                }
                finally
                {
                    client.Close();
                }
            }
        }
    }
}
