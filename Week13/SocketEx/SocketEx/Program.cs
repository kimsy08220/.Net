using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

// socket ���� �� IP�ּ�, ��Ʈ��ȣ ���� ���� connect
// ���̹��� ù �������� html�� ��Ÿ�� ��

namespace SocketEx
{
    class Program
    {
        // �⺻������ ����� ������ ����
        static string _strHostName = "www.naver.com";                                   // == static string _strHostName = "203.241.250.11";
        static string _strPath = "/";                                                   // root

        static void Main(string[] args)
        {
            // ���� ���� (TCP ����) : C# : AddressFamily.InterNetwork == C : AF_INET
            Socket sckTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   
  
            //DNS ���� �˾ƺ� Resolve() �޼ҵ带 ����Ͽ� Host�� ���� ������ ������
            IPHostEntry ipEntry = Dns.Resolve(_strHostName);                              // IPHostEntry : ������ �̸��� �ָ� ������ IP ������ �޾ƿ�

            // ������ �ּ� �� ù��° �����ǿ� 80�� ��Ʈ�� ����� ���� ���� 
            IPEndPoint ipEndPoint = new IPEndPoint(ipEntry.AddressList[0], 80);           // IPEndPoint : ��Ʈ��ũ EndPoint�� IP �ּҿ� ��Ʈ ��ȣ�� ��Ÿ��, ipEntry.AddressList[0] : 2���� naver �ּ� �� ó�� �ּ�, 80 : htttp ����
            IPEndPoint ipEndPoint1 = new IPEndPoint(IPAddress.Parse(textBox?.Text), 80);
            // 127.0.0.1, send receive ȣ��x
            try
            {
                // Connect() : ������ ������ ������ ������ �õ�
                sckTcp.Connect(ipEndPoint);

                // Connected : ������ ����Ǿ��ٸ�(true) 
                if (sckTcp.Connected)
                {
                    Console.WriteLine("������ ����Ǿ����ϴ�. ");

                    // ������ ���ڷ� Http ���������� ����� ����� ������ socketSendReceive �޼ҵ带 ȣ��
                    string strContent = socketSendReceive(sckTcp);

                    // ���� ���� �ҽ��� ������ ���
                    Console.WriteLine(strContent);
                }
                else    // ������� �ʾҴٸ�(false)
                    Console.WriteLine("������ ������ ������� �ʾҽ��ϴ�. ");
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

        // ������ ��û�޼ҵ� Get �޼ҵ� ���ڿ��� �����Ͽ� ���� �������� ������ ��û�ϰ� ���� ���� �޼����� �������� ������ ������
        private static string socketSendReceive(Socket s)
        {
            // �������� ���� �޼ҵ� ���ڿ��� ���� 
            string Get = "GET " + _strPath + " HTTP/1.1\r\n Host: " + _strHostName + "\r\nConnection: Close\r\n\r\n";

            Encoding ASCII = Encoding.ASCII;                  // C# : �����ڵ� ���, �����ڵ带 �ƽ�Ű �ڵ� ���ڿ��� ��ȯ
            Byte[] ByteGet = ASCII.GetBytes(Get);             // ��ǻ�� ���� : Byte ���, ASCII ���ڵ��� ����Ͽ� Byte �迭�� ��ȯ

            // ������ ���� ����Ʈ �迭�� ����
            Byte[] RecvBytes = new Byte[1024];
            String strRetPage = null;

            if (s == null)
                return ("Connection failed");

            // Send() : �������� ��û ����� ����
            s.Send(ByteGet);

            // Receive() : ���� Ȩ�������� ������ ����
            Int32 bytes = s.Receive(RecvBytes, RecvBytes.Length, SocketFlags.None);

            // ó�� 1024 bytes�� ����
            strRetPage = "Default HTML page on " + _strHostName + ":\r\n";
            strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, bytes);   // ASCII.GetString() : ASCII ���ڵ��� ����Ͽ� String �迭�� ��ȯ

            // ó�� ������ �ִ��� Ȯ���� �� ���� ������ ���������� �ݺ������� ����
            while (bytes > 0)
            {
                bytes = s.Receive(RecvBytes, RecvBytes.Length, 0);
                strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, bytes);
            }

            return strRetPage;
        }   
    }
}
