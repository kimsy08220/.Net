using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1",9000); // create client socket, try to automatically connecting

            if (client.Connected) //if server connected return true
            {
                Byte[] sendByte = new Byte[1024];
                NetworkStream ns = client.GetStream();

                 //needed byte type to send data
               
                string send_msg,receive_msg;                       
                while (true)
                {
                    
                    Console.Write("[보낼메시지] : ");
                    send_msg = Console.ReadLine();
                    sendByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(send_msg);
                    ns.Write(sendByte, 0, sendByte.Length);
                    Console.WriteLine(sendByte.Length + "바이트를 전송 하였습니다.");

                    Byte[] receiveByte = new Byte[1024];
                    ns.Read(receiveByte, 0, receiveByte.Length);
                    receive_msg = Encoding.Default.GetString(receiveByte).Trim('\0');
                    receiveByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(receive_msg); //받은 바이트를 문자열로 하여 null 값을 지우고 다시 바이트 형식 변환
                    Console.WriteLine("[받은메시지] : "+receive_msg);
                    Console.WriteLine(receiveByte.Length + "바이트를 전송 받았습니다.\n");
                }

            }

        }
    }
}
