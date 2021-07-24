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
        private Color DialogPenColor;

        public Color color
        {
            get
            {
                if (radioButton1.Checked) DialogPenColor = Color.Red;
                if (radioButton2.Checked) DialogPenColor = Color.Green;
                if (radioButton3.Checked) DialogPenColor = Color.Blue;
                return DialogPenColor;
            }
            set
            {
                DialogPenColor = value;
                if (DialogPenColor == Color.Red) radioButton1.Checked = true;
                if (DialogPenColor == Color.Green) radioButton2.Checked = true;
                if (DialogPenColor == Color.Blue) radioButton3.Checked = true;
            }
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
