using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ComboBoxTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("10");
            comboBox1.Items.Add("20");
            comboBox1.Items.Add("30");
            //defaut: sorted option = false
            comboBox1.SelectedIndex = 0;
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)     // 번개질
        {
            this.Text = Convert.ToString(comboBox1.SelectedIndex);      // Text = comboBox1.Text;와 동일, comboBox1을 선택하면 caption에 index가 출력, this : Form1
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            Text = comboBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}