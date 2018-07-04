using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ruby.Data
{
    public class CodeType
    {
        int _typeId;
        string _typeDefn, _typeDesc, _typeName;
        List<Code> _codes = new List<Code>();

        public CodeType(DataRow row, DataTable dt)
        {
            _typeId = Convert.ToInt32(row[0]);
            _typeDefn = row[1].ToString();
            _typeName = row[2].ToString();
            _typeDesc = row[3].ToString();

            foreach (DataRow r in dt.Rows)
            {
                _codes.Add(new Code(r));
            }
        }

        public int CodeTypeId { get { return _typeId; } }
        public string TypeDefn { get { return _typeDefn; } }
        public string TypeName { get { return _typeName; } }
        public string TypeDesc { get { return _typeDesc; } }

        public List<Code> Codes { get { return _codes; } }
    }
}
