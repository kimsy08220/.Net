using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mixControl
{
    public partial class Form2 : Form
    {
        private int iDialogShape;
        

        public int Shape
        {
            get {
                if (radioButton1.Checked) iDialogShape = 0;
                if (radioButton2.Checked) iDialogShape = 1;
                if (radioButton3.Checked) iDialogShape = 2; 
                return iDialogShape;
            }
            set { iDialogShape=value;
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

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
  
    }
}
