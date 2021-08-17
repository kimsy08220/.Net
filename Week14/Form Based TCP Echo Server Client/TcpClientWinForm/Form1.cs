using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;                   // TcpListener를 사용하기 위해 선언

namespace _190530_TcpClientWinForm
{
    public partial class Form1 : Form
    {
        TcpClient tclient;                  // TCP 네트워크 서비스에 대한 클라이언트 연결을 제공
            
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)      // 연결 버튼을 누르면
        {
            if (button1.Text.Equals("연결"))
            {
                try
                {
                    tclient = new TcpClient(textBox1.Text, int.Parse(textBox2.Text));       // TcpClient(생성과 동시에 연결됨) 생성, (IP 주소, 포트 번호)

                    if (tclient.Connected)
                    {
                        listBox1.Items.Add("소켓이 연결되었습니다.");
                        button1.Text = "연결 해제";
                    }
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add("Exception Thrown : " + ex.ToString());
                    listBox1.Items.Add("소켓이 연결되지 않았습니다.");
                    button1.Text = "연결";
                }
            }
            else
            {
                try
                {
                    button1.Text = "연결";
                    tclient.Close();
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add("Exception Thrown : " + ex.ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)      // 전송 버튼을 누르면
        {
            try
            {
                if (!textBox3.Text.Equals("") && tclient.Connected) // 입력하고 연결되었을 때
                {
                    NetworkStream ns = tclient.GetStream();         // tclient 객체의 GetStream() 메소드로부터 NetworkStream 형식의 객체를 가져와서 데이터를 쓰고 읽음 

                    Encoding ASCII = Encoding.ASCII;
                    Byte[] ByteGet = ASCII.GetBytes(textBox3.Text); // ASCII == UTF8, 문자열을 아스키 코드로 변환 후 byte로 변환
                    Byte[] RecvBytes = new Byte[1024];

                    if (tclient == null)
                    {
                        listBox1.Items.Add("연결 실패");
                        button1.Text = "연결";
                    }
                    else
                    {
                        ns.Write(ByteGet, 0, ByteGet.Length);                   // Write() : Server로 메시지 내용을 전송
                        listBox1.Items.Add("[TCP 클라이언트] " + ByteGet.Length + " 바이트를 보냈습니다.");

                        int bytes = ns.Read(RecvBytes, 0, RecvBytes.Length);    // Read() : Server로부터 메세지 내용을 얻어옴
                        if (bytes == -1) listBox1.Items.Add("메시지 받기 실패");
                        else
                        {
                            listBox1.Items.Add("[TCP 클라이언트] " + bytes + " 바이트를 받았습니다.");
                            listBox1.Items.Add("[Server] : " + ASCII.GetString(RecvBytes, 0, bytes));
                            textBox3.Text = "";
                        }
                    }
                }
                else if (!tclient.Connected)
                {
                    listBox1.Items.Add("이전에 소켓이 연결되지 않았습니다.");
                    button1.Text = "연결";
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Exception Thrown : " + ex.ToString());
                button1.Text = "연결";
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)      // 글자를 누르면
        {
            if (e.KeyChar == (char)Keys.Enter)                                  // Enter 키를 눌러도 전송 버튼을 누를 때랑 동일하게 해줌
                button2_Click(sender, e);
        }
    }
}
