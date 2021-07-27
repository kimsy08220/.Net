using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\";          // 초기 디렉터리를 가져오거나 설정
            openFileDialog1.Title = "파일을 잽싸게 고르셔";     // Title 설정
            openFileDialog1.Filter = "텍스트 파일|*.txt|모든 파일|*.*";  // OpenFileDialog 또는 SaveFileDialog에 표시되는 파일 형식을 결정하는 필터 문자열을 가져오거나 설정
            openFileDialog1.ShowReadOnly = true;                // 대화 상자에 읽기 전용 확인란이 있으면 true 없으면 false
            openFileDialog1.Multiselect = true;                 // 대화 상자에서 동시에 여러 파일을 선택할 수 있으면 true 선택할 수 없으면 false


            //if (openFileDialog1.ShowDialog() == DialogResult.OK)      // 복수 선택
            //{
            //    foreach (string file in openFileDialog1.FileNames)    
            //    {
            //        MessageBox.Show(file + "를 선택했습니다.");
            //    }
            
            //}

            if (openFileDialog1.ShowDialog() == DialogResult.OK)        // 단일 선택
            {
                MessageBox.Show(openFileDialog1.FileName + "를 선택했습니다.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}