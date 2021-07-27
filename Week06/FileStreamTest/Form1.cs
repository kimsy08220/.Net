using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileStreamTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = { 65, 66, 67, 68, 69, 70, 71, 72 };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //FileStream fs = new FileStream(@"c:\temp\fs.txt", FileMode.Create, FileAccess.Write);         // 특정 위치에 저장
                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);    // FileMode.Create : 새 파일 생성, 있으면 덮어씀, FileAccess.Write : 쓰기 권한 부여
                fs.Write(data, 0, data.Length);         // FileStream은 바이트의 연속이므로 바이트 단위의 data만 허용, (쓸 내용, 시작 위치  , 총 갯수)
                fs.Close();                             // 종료
                MessageBox.Show(saveFileDialog1.FileName+ " 파일에 기록했습니다.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[8];

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Read);   // FileMode.OpenOrCreate : 파일이 있으면 열고 없으면 파일 생성, FileAccess.Read : 읽기 권한 부여
                    fs.Read(data, 0, data.Length);      // (읽을 내용, 시작 위치 , 총 갯수)
                    fs.Close();                         // 종료

                    string result = "";
                    for (int i = 0; i < data.Length; i++)
                        result += data[i].ToString() + ",";
                    MessageBox.Show(result, "파일 내용");
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("지정한 파일이 없습니다.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}