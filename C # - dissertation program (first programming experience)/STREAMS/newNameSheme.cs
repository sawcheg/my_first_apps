using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace STREAMS
{
    public partial class newNameSheme : Form
    {
        public newNameSheme(IForm1 io)
        {
            InitializeComponent();
            FirstForm = io;
        }
        IForm1 FirstForm;
        private void nameSheme_Shown(object sender, EventArgs e)
        {
           textBox1.Clear();
           ActiveControl = textBox1;         
        }
        private void button1_Click(object sender, EventArgs e)
        {            
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Вы не ввели ни одного символа, чтобы назвать схему.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Hide();
                    FirstForm.createNewSheme(textBox1.Text);
                    Dispose();
                }
          }
        private void newNameSheme_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        
    }
}
