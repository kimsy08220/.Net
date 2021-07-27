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
            int[] data = { 65, 66, 67, 68, 69, 70, 71, 72 };

            FileStream fs = new FileStream(@"c:\temp\fs.txt", FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);     // BinaryWriter의 도움으로 byte형 변환
            for (int i=0; i < data.Length; i++ )
                bw.Write(data[i]);
            fs.Close();
            MessageBox.Show(@"C:의 fs.txt 파일에 기록했습니다.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] data = new int[8];

            try
            {
                FileStream fs = new FileStream(@"c:\temp\fs.txt", FileMode.OpenOrCreate, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs); // BinaryWriter의 도움으로 byte형 변환
                for (int i = 0; i < data.Length; i++)
                    data[i] = br.ReadInt32();           // byte형을 int형으로 변환해서 data에 저장
                fs.Close();

                string result = "";
                for (int i = 0; i < data.Length; i++)
                    result += data[i].ToString() + ",";
                MessageBox.Show(result, "파일 내용");
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