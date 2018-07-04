using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Excel;
using System.IO;

namespace SaveWorkbookInStream
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DataTable table = new DataTable();
            table.Columns.Add("Col1", typeof(string));
            for (int i = 0; i < 10; i++)
                table.Rows.Add("Test Text" + i.ToString());
            ultraGrid1.DataSource = table;
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            Workbook book = new Workbook();
            ultraGridExcelExporter1.Export(ultraGrid1, book);
            MemoryStream s = new MemoryStream();
            book.Save(s);

            richTextBox1.LoadFile(s, RichTextBoxStreamType.PlainText);

        }

       
    }
}
