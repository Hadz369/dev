using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using Newtonsoft.Json;
using ProtoBuf;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSheet timesheet = new TimeSheet();
            //timesheet.Load("Timesheet Weekly_25-04-2016 Ian.xlsx");
            //timesheet.Load("Timesheet Weekly_20160627.xlsx");
            //timesheet.Load("Timesheet Weekly_20160411_QuangDang.xlsx");
            //timesheet.Load("Timesheet Weekly_20160815.xlsx");
            timesheet.Load("Timesheet Weekly_20160801_QuangDang.xlsx");
            

            TimeSheetValidator.Validate(timesheet);

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nEnd of program");
            Console.ReadLine();
        }

        string ToHexString(TimeSheet timesheet)
        {

            MemoryStream ms = new MemoryStream();

            Serializer.Serialize<TimeSheet>(ms, timesheet);
            byte[] bytes = ms.ToArray();

            StringBuilder sb = new StringBuilder("Bytes:");

            foreach (byte b in bytes)
            {
                sb.Append(" ");
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }
    }

    [ProtoContract]
    class TimeSheet
    {
        string _name;
        DateTime _dateFrom, _dateTo;
        List<TimeSheetDay> _days = new List<TimeSheetDay>();

        [ProtoMember(5)]
        public string Name { get { return _name; } set { _name = value; } }
        
        [ProtoMember(10)]
        public DateTime DateFrom { get { return _dateFrom; } set { _dateFrom = value; } }
        
        [ProtoMember(15)]
        public DateTime DateTo { get { return _dateTo; } set { _dateTo = value; } }

        [ProtoMember(20)]
        public List<TimeSheetDay> Days { get { return _days; } set { _days = value; } }

        public void Load(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            MemoryStream msFirstPass = new MemoryStream();
            SLDocument summary = new SLDocument(fs, "Summary");
            
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;

            _name     = summary.GetCellValueAsString(3, 2);
            _dateFrom = summary.GetCellValueAsDateTime(4, 2);
            _dateTo   = summary.GetCellValueAsDateTime(5, 2);

            Console.WriteLine("Name={0}, From={1}, To={2}\n", _name, _dateFrom, _dateTo );

            for (int x = 8; x < 15; x++)
            {
                string   day   = summary.GetCellValueAsString(x, 1);
                DateTime date  = summary.GetCellValueAsDateTime(x, 2);
                decimal  hours = summary.GetCellValueAsDecimal(x, 3);

                TimeSheetDay tsd = new TimeSheetDay(day, date, hours);

                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Day={0}, Date={1}, Hours={2}", day, date, hours);


                ProcessDay(tsd, new SLDocument(fs, day));
                _days.Add(tsd);

                Console.WriteLine();

                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue;
            }

        }

        static void ProcessDay(TimeSheetDay timesheetDay, SLDocument sheet)
        {
            decimal runningTotal = 0;

            for (int x = 4; x < 100; x++)
            {
                var c1 = sheet.GetCellValueAsDecimal(x, 1);
                var c2 = sheet.GetCellValueAsString(x, 2);

                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;

                if (c2.ToLower() == "total")
                {
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Magenta;

                    Console.WriteLine("Total={0}, Actual={1}, Variance={2}", c1, runningTotal, c1 - runningTotal);
                    break;
                }
                else
                {
                    if (c1 > 0)
                    {
                        runningTotal += c1;

                        TimeSheetEntry entry = new TimeSheetEntry(sheet, x);
                        timesheetDay.Entries.Add(entry);
                        Console.WriteLine("> {0}", entry.ToString());
                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        foreach (string error in TimeSheetValidator.ValidateEntry(entry))
                            Console.WriteLine("> > Error: {0}", error);
                    }
                }
            }
        }
    }

    [ProtoContract]
    class TimeSheetDay
    {
        public TimeSheetDay(string day, DateTime date, decimal hours)
        {
            Day = day;
            Date = date;
            TotalHours = hours;
            Entries = new List<TimeSheetEntry>();
        }

        [ProtoMember(5)]
        public string Day { get; private set; }
        
        [ProtoMember(10)]
        public DateTime Date { get; private set; }
        
        [ProtoMember(15)]
        public decimal TotalHours { get; private set; }
        
        [ProtoMember(20)]
        public List<TimeSheetEntry> Entries { get; private set; }
    }

    [ProtoContract]
    class TimeSheetEntry
    {
        public TimeSheetEntry(SLDocument sheet, int row)
        {
            Hours        = sheet.GetCellValueAsDecimal(row, 1);
            Description  = sheet.GetCellValueAsString(row, 2);
            Type = sheet.GetCellValueAsString(row, 3);
            System = sheet.GetCellValueAsString(row, 4);
            AdminType    = sheet.GetCellValueAsString(row, 5);
            Project      = sheet.GetCellValueAsString(row, 6);
            Product      = sheet.GetCellValueAsString(row, 7);
            IssueList    = sheet.GetCellValueAsString(row, 8);
            Jurisdiction = sheet.GetCellValueAsString(row, 9);
       }

        [ProtoMember(5)]
        public decimal Hours { get; private set; }
        
        [ProtoMember(15)]
        public string Description { get; private set; }
        
        [ProtoMember(20)]
        public string Type { get; private set; }
        
        [ProtoMember(25)]
        public string System { get; private set; }
        
        [ProtoMember(30)]
        public string AdminType { get; private set; }
        
        [ProtoMember(35)]
        public string Project { get; private set; }
        
        [ProtoMember(40)]
        public string Product { get; private set; }
        
        [ProtoMember(45)]
        public string IssueList { get; private set; }
        
        [ProtoMember(50)]
        public string Jurisdiction { get; private set; }

        public override string ToString()
        {
            string str = String.Empty;

            str = String.Concat(
                "Hours=", Hours, ", ",
                "Description=", Description, ", ",
                "Type=", Type, ", ",
                "System=", System, ", ",
                "AdminType=", AdminType, ", ",
                "Project=", Project, ", ",
                "Product=", Product, ", ",
                "IssueList=", IssueList, ", ",
                "Jurisdiction=", Jurisdiction);

            return str;
        }
    }

    static class TimeSheetValidator
    {
        static List<string> _errors = new List<string>();

        public static List<string> Validate(TimeSheet timeSheet)
        {
            _errors.Clear();

            foreach (TimeSheetDay day in timeSheet.Days)
            {
                int count = 0;

                foreach (TimeSheetEntry entry in day.Entries)
                {
                    foreach (string error in ValidateEntry(entry))
                        _errors.Add(String.Format("{0}, {1}, {2}, {3}", day.Day, day.Date.ToShortDateString(), count, error));

                    count++;
                }
            }

            return _errors;
        }

        public static List<string> ValidateEntry(TimeSheetEntry entry)
        {
            List<string> errors = new List<string>();

            string msg = "";

            if (MissingStringValue(entry.Description)) msg = String.Concat(msg, (msg.Trim() == "" ? "" : ", "), "Description");

            if (MissingStringValue(entry.Type))
            {
                msg = String.Concat(msg, (msg.Trim() == "" ? "" : ", "), "Type");
            }
            else if (TypeInvalid(entry.Type))
            {
                errors.Add("Type invalid");
            }

            if (MissingStringValue(entry.System))
            {
                msg = String.Concat(msg, (msg.Trim() == "" ? "" : ", "), "System");
            }
            else if (SystemInvalid(entry.System))
            {
                errors.Add("System invalid");
            }

            if (MissingStringValue(entry.Project))
            {
                msg = String.Concat(msg, (msg.Trim() == "" ? "" : ", "), "Project");
            }

            if (JurisdictionInvalid(entry.Type, entry.Jurisdiction))
            {
                errors.Add("Jurisdiction invalid");
            }

            if (msg.Trim() != String.Empty)
                errors.Add(String.Concat("Missing one or more required values: ", msg));

            return errors;
        }

        static bool MissingStringValue(string value)
        {
            if (value.Trim() == String.Empty)
                return true;
            else
                return false;
        }

        enum TaskType
        {
            Undefined,
            Development,
            Maintenance,
            Support,
            Administration
        }
        
        static bool TypeInvalid(string type)
        {
            try
            {
                Enum.Parse(typeof(TaskType), type, true);
                return false;
            }
            catch 
            { 
                return true; 
            }
        }

        enum System
        {
            Undefined,
            General,
            Metropolis,
            Clubline,
            Sentinel,
            Astute,
            Engage,
            Titan
        }

        static bool SystemInvalid(string system)
        {
            try
            {
                Enum.Parse(typeof(System), system, true);
                return false;
            }
            catch
            {
                return true;
            }
        }

        enum Jurisdiction
        {
            Undefined,
            NSW,
            QLD,
            TAS,
            VIC,
            INT,
        }

        static bool JurisdictionInvalid(string type, string jurisdiction)
        {
            TaskType tt;
            Jurisdiction jj;

            try   { tt = (TaskType)Enum.Parse(typeof(TaskType), type, true); }
            catch { tt = TaskType.Undefined; }

            try   { jj = (Jurisdiction)Enum.Parse(typeof(Jurisdiction), jurisdiction, true); }
            catch { jj = Jurisdiction.Undefined; }

            if (tt == TaskType.Development || tt == TaskType.Maintenance || tt == TaskType.Support)
            {
                if (jj == Jurisdiction.Undefined) // Must have a jurisdiction for development types
                    return true;
                else
                    return false;
            }
            else if (tt == TaskType.Administration)
            {
                if (jj == Jurisdiction.Undefined) 
                    return false;
                else
                    return true; // No jurisdiction should be provided for admin types
            }
            else
            {
                if (jj != Jurisdiction.Undefined)
                    return true; // No type specified with jurisdicyion so return an error
            }

            return false;
        }
    }
}