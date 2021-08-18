using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1
{
    public partial class Form2 : Form
    {
        public int iDialogShape;
        public int iDialogWidth { get; set; }
        public Color iDialogColor { get; set; }
        public int size { get; set; }
        //{
        //    get { return int.Parse(comboBox1.Text); }
        //    set { comboBox1.Text = value.ToString(); }
        //}
        public int shape // 선, 동그라미, 네모 모양
        {
            get
            {
                if (radioButton1.Checked) iDialogShape = 0;
                if (radioButton2.Checked) iDialogShape = 1;
                if (radioButton3.Checked) iDialogShape = 2;
                return iDialogShape;
            }
            set
            {
                iDialogShape = value;
                if (iDialogShape == 0) radioButton1.Checked = true;
                if (iDialogShape == 1) radioButton2.Checked = true;
                if (iDialogShape == 2) radioButton3.Checked = true;
            }
        }
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("6");
            comboBox1.Items.Add("8");
            comboBox1.Items.Add("10");
            //comboBox1.SelectedIndex = 0; //form2창 띄울때 combobox숫자 초기화값을 2로 하는것
            comboBox1.Text = iDialogWidth.ToString();


            if (shape == 0)
                radioButton1.Checked = true;
            else if (shape == 1)
                radioButton2.Checked = true;
            else if (shape == 2)
                radioButton3.Checked = true;

            hScrollBar1.Value = iDialogColor.R;
            hScrollBar2.Value = iDialogColor.G;
            hScrollBar3.Value = iDialogColor.B;
            textBox1.Text = hScrollBar1.Value.ToString();
            textBox2.Text = hScrollBar2.Value.ToString();
            textBox3.Text = hScrollBar3.Value.ToString();
        }
        private void label1_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brc = new SolidBrush(iDialogColor);
            Pen p = new Pen(iDialogColor);

            if (shape == 0)
            {
                e.Graphics.DrawLine(new Pen(iDialogColor, iDialogWidth), 0, 50, 200, 50);
            }
            else if (shape == 1)
            {
                e.Graphics.DrawEllipse(p, 0, 15, size, size); // 
                e.Graphics.FillEllipse(brc, 0, 15, size, size); // 
            }
            else if (shape == 2)
            {
                e.Graphics.DrawRectangle(p, 0, 15, size, size); // 
                e.Graphics.FillRectangle(brc, 0, 15, size, size); // 
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            iDialogWidth = (((ComboBox)sender).SelectedIndex + 1) *2; // combobox안에 숫자로 인해 수식이 바뀐다.
            label1.Invalidate();
        }
        private void hScrollBar_Scroll(object sender, ScrollEventArgs e) //hscrollbar의 변경사항
        {
            textBox1.Text = hScrollBar1.Value.ToString(); // 스크롤바의 변경 값을 textbox의 숫자도 같이 변경해주는것
            textBox2.Text = hScrollBar2.Value.ToString(); // 스크롤바의 변경 값을 textbox의 숫자도 같이 변경해주는것
            textBox3.Text = hScrollBar3.Value.ToString(); // 스크롤바의 변경 값을 textbox의 숫자도 같이 변경해주는것
            //iDialogColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value); // textbox에서 썻기 때문에 사용 하지않아도 됨, 하지만 label을 썻을 때는 사용해야함
            label1.Invalidate();
        }
        private void textBox1_TextChanged_1(object sender, EventArgs e) //textbox1의 변경사항
        {
            hScrollBar1.Value = int.Parse(textBox1.Text);
            textBox1.Text = hScrollBar1.Value.ToString();
            iDialogColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            label1.Invalidate();
        }
        private void comboBox1_TextChanged(object sender, EventArgs e) //combox안에 숫자 입력
        {
            try
            {
                if(comboBox1.Text == "" || int.Parse(comboBox1.Text) < 0)
                {
                    MessageBox.Show("양의정수를 입력해주세요.");
                }
                else
                {
                    iDialogWidth = int.Parse(comboBox1.Text);
                    size = int.Parse(comboBox1.Text);
                }
            }
            catch(FormatException)
            {

            }
            //iDialogWidth = int.Parse(comboBox1.Text);
            label1.Invalidate();
        }
        private void textBox2_TextChanged_1(object sender, EventArgs e) //textbox2의 변경사항
        {
            hScrollBar2.Value = int.Parse(textBox2.Text);
            textBox2.Text = hScrollBar2.Value.ToString();
            iDialogColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            label1.Invalidate();
        }
        private void textBox3_TextChanged_1(object sender, EventArgs e) //textbox3의 변경사항
        {
            hScrollBar3.Value = int.Parse(textBox3.Text);
            textBox3.Text = hScrollBar3.Value.ToString();
            iDialogColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            label1.Invalidate();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Invalidate();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Invalidate();
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label1.Invalidate();
        }
    }
}
