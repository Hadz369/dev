using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using eBet.Core;
using eBet.Data;

namespace eBet.Data
{
    public static class Cache
    {
        static bool _initialised = false;
        static DateTime _lastRefresh;
        static Codes _codes;
        static Guid _guid;

        public static void Initialise(string connectionString)
        {
            _guid = Guid.NewGuid();

            _codes = new Codes();

            //DbConnectionBroker cb = DbConnectionBroker.Instance;
            //cb.Register(_guid, connectionString);

            Refresh();

            _initialised = true;
        }

        public static Codes Codes { get { return _codes; } }

        public static void Refresh()
        {
            DataTable dt = new DataTable();

            /*
            using (DbHandler db = new DbHandler(_guid))
            {
                CoreCommandBuilder cb = new CoreCommandBuilder();

                if (!_initialised) 
                    dt = db.Execute(cb.GetCodes(db, new DateTime(1800,1,1)));
                else
                    dt = db.Execute(cb.GetCodes(db, _lastRefresh));
            }
            */

            _lastRefresh = DateTime.Now;

            AddCodes(dt);
        }

        static void AddCodes(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                Code nc = new Code(row);
                Code xc = _codes.GetCode(nc.CodeTypeDefn, nc.CodeDefn);

                if (xc != null)
                    xc.Update(nc);
                else
                    _codes.Add(nc);
            }
        }
    }

    public class Codes : List<Code>
    {
        public Code GetCode(string type, string code)
        {
            foreach (Code c in this)
            {
                if (String.Compare(type, c.CodeTypeDefn, true) == 0 && String.Compare(code, c.CodeDefn, true) == 0)
                {
                    return c;
                }
            }

            return null;
        }
    }

    public class Code
    {
        int _typeId, _codeId, _sequence;
        string _typeDefn, _codeDefn, _codeName, _codeDesc, _codeValue;

        public Code(DataRow row)
        {
            _typeId = Convert.ToInt32(row[0]);
            _typeDefn = row[1].ToString();
            _codeId = Convert.ToInt32(row[2]);
            _codeDefn = row[3].ToString();
            _codeName = row[4].ToString();
            _codeDesc = row[5].ToString();
            _codeValue = row[6].ToString();
            _sequence = Convert.ToInt32(row[7]);

        }

        public int CodeTypeId { get { return _typeId; } }
        public string CodeTypeDefn { get { return _typeDefn; } }
        public int CodeId { get { return _codeId; } }
        public string CodeDefn { get { return _codeDefn; } }
        public string CodeName { get { return _codeName; } }
        public string CodeDesc { get { return _codeDesc; } }
        public string CodeValue { get { return _codeValue; } }
        public int Sequence { get { return _sequence; } }

        public void Update(Code newcode)
        {
            _codeName  = newcode.CodeName;
            _codeDesc  = newcode.CodeDesc;
            _codeValue = newcode.CodeValue;
            _sequence  = newcode.Sequence;
        }
    }
}
