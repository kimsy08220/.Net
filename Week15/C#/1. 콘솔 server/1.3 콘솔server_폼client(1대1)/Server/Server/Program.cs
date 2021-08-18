using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets; //TcpListener
using System.Net;           // IPEndPoint
using System.IO;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress host_ip = IPAddress.Parse("127.0.0.1"); //host 아이피 설정?
            int port_num = 9000;                               
            IPEndPoint serverEndPoint = new IPEndPoint(host_ip, port_num); //???

            TcpListener listener = new TcpListener(serverEndPoint); //서버 소켓생성 TCP용
            listener.Start();                                       //서버 시작

            TcpClient client = listener.AcceptTcpClient();          //서버에 접속한 TCP 소켓을 저장

            if (client.Connected) //접속되어 있으면 true 값이 존재
            {
                Console.WriteLine("클라이언트가 접속하였습니다.");
                String msg;
                 //needed byte type to send data
            
                NetworkStream ns = client.GetStream();              //파일클래스를 쓰기위해 스트림을 받아옴
    
                while (true)
                {
                    Byte[] msgByte = new Byte[1024];
                    ns.Read(msgByte, 0, msgByte.Length);
                    msg = Encoding.Default.GetString(msgByte).Trim('\0');
                    msgByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(msg); //받은 바이트를 문자열로 하여 null 값을 지우고 다시 바이트 형식 변환
                    Console.WriteLine("[받은메시지] : " + msg);
                    Console.WriteLine(msgByte.Length + "바이트를 전송 받았습니다.\n");

                    ns.Write(msgByte, 0, msgByte.Length);
                }
            }
 

        }
    }
}
