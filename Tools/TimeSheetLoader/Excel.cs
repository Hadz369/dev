using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Infragistics.Excel;

namespace TimeSheetLoader
{
    public static class Excel
    {
        public static Dictionary<string, List<DataColumn>> Columns = new Dictionary<string, List<DataColumn>>();

        public static string LoadTimeSheet(string fileName)
        {
            DataSet ds = new DataSet();

            Workbook book = Workbook.Load(fileName);
            Worksheet sheet = book.Worksheets[0];

            string msg = "";

            msg += sheet.Rows[2].Cells[1].Value.ToString() + "\n";
            msg += sheet.Rows[3].Cells[1].Value.ToString() + "\n";

            for (int x = 6; x < 14; x++)
            {
                msg += String.Format("Day={0}|Desc={1}|Hours={2}\n",
                    sheet.Rows[x].Cells[0].Value,
                    sheet.Rows[x].Cells[1].Value,
                    sheet.Rows[x].Cells[2].Value);
            }

            return msg;
        }
    }
}
