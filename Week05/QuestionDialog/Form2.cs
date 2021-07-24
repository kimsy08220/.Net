using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuestionDialog
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

private void button1_Click(object sender, EventArgs e)
{
    //DialogResult = DialogResult.Yes;      // 여기서 설정해도 되고 속성창에서 설정해줘도 됨
}

private void button2_Click(object sender, EventArgs e)
{
    //DialogResult = DialogResult.No;
}

private void Form2_Load(object sender, EventArgs e)
{

}
    }
}