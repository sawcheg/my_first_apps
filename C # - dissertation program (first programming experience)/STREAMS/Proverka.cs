using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace STREAMS
{
    public partial class Proverka : Form
    {
        public Proverka()
        {
            InitializeComponent();            
        }        
        private void button1_Click(object sender, EventArgs e)
        {
                this.Close();
        }      
        public void name_button()
        {  button1.Text = "Продолжить";}
        public void AddColoredText(string text, Color color, bool bold)
        {
            this.Otchet.Select(0, 0);
            int startPosition = this.Otchet.Text.Length;
            this.Otchet.AppendText(text);
            this.Otchet.Select(startPosition, text.Length);
            if (bold)
                Otchet.SelectionFont = new Font(Otchet.Font.FontFamily, this.Font.Size, FontStyle.Bold);
            else
                Otchet.SelectionFont = new Font(Otchet.Font.FontFamily, this.Font.Size);
            this.Otchet.SelectionColor = color;
            this.Otchet.Select(0, 0);

        }
        

    }
   
}
