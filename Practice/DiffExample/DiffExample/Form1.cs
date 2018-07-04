using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiffExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string text = "Some Default" + Environment.NewLine + "Text";

            richTextBox1.Text = text;
            richTextBox2.Text = text;
        }

        private void CompareText()
        {
            int pos = 0;
            string a_line = richTextBox1.Text;
            string b_line = richTextBox2.Text;

            Diff.Item[] diffs = Diff.DiffInt(DiffCharCodes(a_line, false), DiffCharCodes(b_line, false));

            for (int n = 0; n < diffs.Length; n++)
            {
                Diff.Item it = diffs[n];

                // loop through characters until a difference is found
                while ((pos < it.StartB) && (pos < b_line.Length))
                {
                    //this.Response.Write(b_line[pos]);
                    pos++;
                } // while

                // write deleted chars
                if (it.deletedA > 0)
                {
                    for (int m = 0; m < it.deletedA; m++)
                    {
                        richTextBox1.SelectionStart = it.StartA + m;
                        richTextBox1.SelectionLength = 1;
                        richTextBox1.SelectionBackColor = Color.LightBlue;
                    }
                }

                // write inserted chars
                if (pos < it.StartB + it.insertedB)
                {
                    while (pos < it.StartB + it.insertedB)
                    {
                        richTextBox2.SelectionStart = pos;
                        richTextBox2.SelectionLength = 1;
                        richTextBox2.SelectionBackColor = Color.LightPink;
                        pos++;
                    }
                }
            }

            // write rest of unchanged chars
            while (pos < b_line.Length)
            {
                //richTextBox1.AppendText(b_line[pos].ToString());
                //this.Response.Write(b_line[pos]);
                pos++;
            } // while
        }

        private static int[] DiffCharCodes(string aText, bool ignoreCase)
        {
            int[] Codes;

            if (ignoreCase)
                aText = aText.ToUpperInvariant();

            Codes = new int[aText.Length];

            for (int n = 0; n < aText.Length; n++)
                Codes[n] = (int)aText[n];

            return (Codes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.DeselectAll();
            richTextBox2.DeselectAll();

            CompareText();
        }
    }
}
