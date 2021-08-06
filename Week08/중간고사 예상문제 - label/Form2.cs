using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 중간고사_예상문제
{
    public partial class Form2 : Form
    {
        public int iDialogShape;
        public Color DialogColor { get; set; }
        public int iDialogWidth { get; set; }
        public int iDialogSize { get; set; }
        //{
        //    get { return Convert.ToInt32(comboBox1.Text); }
        //    set { comboBox1.Text = value.ToString(); }
        //}
        public int shape
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
            for (int i = 2; i <= 10; i += 2)
                comboBox1.Items.Add(i);
            comboBox1.Text = iDialogWidth.ToString();

            if (shape == 0)
                radioButton1.Checked = true;
            else if (shape == 1)
                radioButton2.Checked = true;
            else if (shape == 2)
                radioButton3.Checked = true;

            hScrollBar1.Value = DialogColor.R;
            hScrollBar2.Value = DialogColor.G;
            hScrollBar3.Value = DialogColor.B;

            label1.Text = DialogColor.R.ToString();
            label2.Text = DialogColor.G.ToString();
            label3.Text = DialogColor.B.ToString();
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            DialogColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            label1.Text = hScrollBar1.Value.ToString();
            label2.Text = hScrollBar2.Value.ToString();
            label3.Text = hScrollBar3.Value.ToString();
            label4.Invalidate();
        }

        private void label4_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brc = new SolidBrush(DialogColor); 
            Pen p = new Pen(DialogColor);

            if (shape == 0)
            {
                e.Graphics.DrawLine(new Pen(DialogColor, iDialogWidth), 0, label4.Height / 2, 700, label4.Height / 2);
            }
            else if (shape == 1) 
            {
                e.Graphics.DrawEllipse(p, 5, 5, iDialogSize, iDialogSize);  
                e.Graphics.FillEllipse(brc, 5, 5, iDialogSize, iDialogSize); 
            }
            else if (shape == 2) 
            {
                e.Graphics.DrawRectangle(p, 5, 5, iDialogSize, iDialogSize); 
                e.Graphics.FillRectangle(brc, 5, 5, iDialogSize, iDialogSize); 
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label4.Invalidate();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label4.Invalidate();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label4.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            iDialogWidth = (((ComboBox)sender).SelectedIndex + 1) * 2;
            iDialogSize = (((ComboBox)sender).SelectedIndex + 1) * 2;                   // iDialogSize get,set을 따로 지정해주지 않으면 이걸 써줘야 함
            label4.Invalidate();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            //iDialogWidth = int.Parse(comboBox1.Text);
            //iDialogSize = int.Parse(comboBox1.Text);
            //label4.Invalidate();

            try
            {
                if (comboBox1.Text == "" || int.Parse(comboBox1.Text) < 0)
                {
                    MessageBox.Show("올바른 숫자를 입력해주세요.");
                }
                else
                {
                    iDialogWidth = int.Parse(comboBox1.Text);
                    iDialogSize = int.Parse(comboBox1.Text);                            // iDialogSize get,set을 따로 지정해주지 않으면 이걸 써줘야 함
                }
            }
            catch (FormatException) { }
            label4.Invalidate();
        }
    }
}
