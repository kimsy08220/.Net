using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace WinTCPClient
{
    class Program
    {
        static string _strHostIP = "127.0.0.1";

        static void Main(string[] args)
        {
            TcpClient tclient = new TcpClient(_strHostIP, 9000);        // TcpClient : 소켓 생성 후 connect가 자동으로 완성
            string send_str;
            byte[] recev = new byte[513];

            try
            {
                // TcpClient를 생성하면서 연결되었으므로 연결 부분이 불필요

                // 소켓이 연결되었다면(true)
                if (tclient.Connected)
                {
                    Console.WriteLine("소켓이 연결되었습니다. ");
                    NetworkStream ns = tclient.GetStream();                     // TcpClient 객체의 GetStream() 메소드로부터 NetworkStream 형식의 객체를 가져와서 데이터를 쓰고 읽음    

                    while (true)
                    {
                        Console.Write("보낼 데이터 : ");
                        send_str = Console.ReadLine();

                        if (send_str == null)
                            break;
                        byte[] sendbyte = Encoding.ASCII.GetBytes(send_str);    // ASCII == UTF8, 문자열을 아스키 코드로 변환 후 byte로 변환

                        // Write() : 서버로 메시지 내용을 바이트로 전송                   
                        ns.Write(sendbyte, 0, sendbyte.Length);                 // Write() : stream의 메소드, send -> write
                        Console.WriteLine("[TCP 클라이언트] {0}바이트를 보냈습니다.", sendbyte.Length);

                        // Read() : 서버로부터 메세지 내용을 받음
                        int recevbyte = ns.Read(recev, 0, recev.Length);        // Read() : stream의 메소드, receive -> read
                        
                        if (recevbyte == -1)
                        {
                            Console.WriteLine("메시지 받기 실패");
                            break;
                        }

                        string recevstr = Encoding.ASCII.GetString(recev, 0, recevbyte);    // ASCII == Default, ASCII.GetString() : ASCII 인코딩을 사용하여 String 배열로 변환
                        Console.WriteLine("[TCP 클라이언트] {0}바이트를 받았습니다..", recevbyte);
                        Console.WriteLine("다시 받은 데이터 : {0}", recevstr);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Thrown : " + ex.ToString());
            }
            finally
            {
                tclient.Close();
            }
        }
    }
}
