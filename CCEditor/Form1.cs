using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;

namespace CCEditor
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.materialMultiLineTextBox1.AppendText("==============[CC Editor Started]=============");
        }

        private void languageBtn_Click(object sender, EventArgs e)
        {

            
        }
    }
}
