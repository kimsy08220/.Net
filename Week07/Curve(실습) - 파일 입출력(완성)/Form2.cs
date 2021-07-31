using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Curve
{
    public partial class Form2 : Form
    {
        public Color DialogPenColor { get; set; }
        public int iDialogPenWidth { get; set; }

        public Form2()      // 객체초기화
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)     // 초기값 설정
        {
            for (int i = 2; i <= 10; i += 2)
            {
                comboBox1.Items.Add(i);
                //comboBox1.SelectedIndex = DialogPenWidth / 2 - 1;
            }
            comboBox1.Text = iDialogPenWidth.ToString();

            hScrollBar1.Value = DialogPenColor.R;
            hScrollBar2.Value = DialogPenColor.G;
            hScrollBar3.Value = DialogPenColor.B;

            label2.Text = "R";
            label3.Text = "G";
            label4.Text = "B";

            textBox1.Text = DialogPenColor.R.ToString();
            textBox2.Text = DialogPenColor.G.ToString();
            textBox3.Text = DialogPenColor.B.ToString();

            //comboBox1.SelectedIndex = 0;

        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            DialogPenColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            textBox1.Text = hScrollBar1.Value.ToString();
            textBox2.Text = hScrollBar2.Value.ToString();
            textBox3.Text = hScrollBar3.Value.ToString();
            label5.Invalidate();
        }

        // label5(미리보기 창) : AutoSize = false 변경 후 사이즈 변경
        private void label5_Paint(object sender, PaintEventArgs e)                  // label5가 다시 그려질 때 호출
        {
            //Graphics G = CreateGraphics();          // label5가 아닌 Form의 크기를 얻어오기 때문에 e.Graphics 선언해야 함
            e.Graphics.DrawLine(new Pen(DialogPenColor, iDialogPenWidth), 0, label5.Height / 2, 700, label5.Height / 2);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)   // 선택 기능
        {
            iDialogPenWidth = (((ComboBox)sender).SelectedIndex + 1) * 2;
            label5.Invalidate();
        }

        private void comboBox1_TextChanged_1(object sender, EventArgs e)            // 직접 입력 기능, comboBox1_SelectedIndexChanged에서 안 하고 comboBox1_TextChanged에서 해줘도 됨
        {
            iDialogPenWidth = int.Parse(comboBox1.Text);
            label5.Invalidate();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
