using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.Collections;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Server
{
    class Program
    {

        private static ArrayList clientList = new ArrayList();

        static void Main(string[] args)
        {
            IPAddress host_ip = IPAddress.Parse("127.0.0.1"); //host 아이피 설정?
            int port_num = 9000;
            IPEndPoint serverEndPoint = new IPEndPoint(host_ip, port_num); //???

            TcpListener listener = new TcpListener(serverEndPoint); //서버 소켓생성 TCP용
            listener.Start();                                       //서버 시작

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();          //서버에 접속한 TCP 소켓을 저장
                clientList.Add(client);

                Thread thread = new Thread(new ParameterizedThreadStart(ClientThread)); //서버를 시작함.
                thread.Start(client);
            }

        }

        static void ClientThread(Object sender)
        {
            TcpClient client;
            client = (TcpClient)sender;
            NetworkStream ns;
            string nick;
            if (client.Connected) //접속되어 있으면 true 값이 존재
            {
                String msg;
                //needed byte type to send data
                ns = client.GetStream();              //파일클래스를 쓰기위해 스트림을 받아옴


                //client 이름을 뿌려줘야함
                Byte[] msgByte = new Byte[1024];
                ns.Read(msgByte, 0, msgByte.Length);
                nick = Encoding.Default.GetString(msgByte).Trim('\0');
                Console.WriteLine(nick + " 님이 접속 하셨습니다.");

                BroadCast(Encoding.GetEncoding("ks_c_5601-1987").GetBytes(nick + " 님이 접속 하셨습니다.\n"));
                try
                {
                    while (true)
                    {
                        msgByte = new Byte[1024];
                        int i = ns.Read(msgByte, 0, msgByte.Length);

                        msg = Encoding.Default.GetString(msgByte).Trim('\0');
                        msgByte = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(msg); // 받은 바이트를 문자열로 하여 null 값을 지우고 다시 바이트 형식 변환
                        Console.WriteLine("[받은메시지]\r\n" + msg);
                        Console.WriteLine(msgByte.Length + "바이트를 전송 받았습니다.\n");
                        BroadCast(msgByte);
                    }
                }
                catch (IOException) 
                {
                    ns.Close();
                    client.Close();
                    Console.WriteLine(nick + " 님이 나갔습니다.");
                    BroadCast(Encoding.GetEncoding("ks_c_5601-1987").GetBytes(nick + " 님이 나갔습니다."));
                }
                catch (SocketException) { }
            }
        }

        public static void BroadCast(Byte[] msgByte)
        {
            NetworkStream ns;
            foreach (TcpClient client in clientList)
            {
                if (client.Connected)
                {
                    ns = client.GetStream();
                    ns.Write(msgByte, 0, msgByte.Length);
                }
            }
        }
    }
}
