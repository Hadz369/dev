using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;

namespace HS
{
    [DataContract]
    public class Code
    {
        public Code() { }

        public Code(DataRow row)
        {
            CodeTypeId = Convert.ToInt32(row[0]);
            CodeTypeDefn = row[1].ToString().Trim();
            CodeGuid = (Guid)row[2];
            CodeId = Convert.ToInt32(row[3]);
            CodeDefn = row[4].ToString();
            CodeName = row[5].ToString();
            CodeDesc = row[6].ToString();
            CodeValue = row[7].ToString();
            Sequence = Convert.ToInt32(row[8]);

        }

        [DataMember]
        public int    CodeTypeId { get; set; }
        [DataMember]
        public string CodeTypeDefn { get; set; }
        [DataMember]
        public int    CodeId { get; set; }
        [DataMember]
        public Guid   CodeGuid { get; set; }
        [DataMember]
        public string CodeDefn { get; set; }
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string CodeDesc { get; set; }
        [DataMember]
        public string CodeValue { get; set; }
        [DataMember]
        public int    Sequence { get; set; }

        public void Update(Code newcode)
        {
            CodeName = newcode.CodeName;
            CodeDesc = newcode.CodeDesc;
            CodeValue = newcode.CodeValue;
            Sequence = newcode.Sequence;
        }
    }

    [CollectionDataContract(Namespace = "", Name = "CodeCollection", ItemName = "Code")]
    public class CodeCollection : List<Code> { }

    public class CodeType
    {
        int _typeId;
        string _typeDefn, _typeDesc, _typeName;
        CodeCollection _codes = new CodeCollection();

        public CodeType() { }

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

        public CodeCollection Codes { get { return _codes; } }
    }

    public class CodeCache
    {
        DateTime _refreshed = new DateTime(1800, 1, 1);
        List<Code> _codes = new List<Code>();

        // Different dictionaries for faster searches
        SortedDictionary<int, Code> _iList = new SortedDictionary<int, Code>();
        SortedDictionary<Guid, Code> _gList = new SortedDictionary<Guid, Code>();
        SortedDictionary<string, CodeCollection> _tList = new SortedDictionary<string, CodeCollection>();

        public DateTime LastRefresh { get { return _refreshed; } }

        public void SetRefreshed()
        {
            _refreshed = DateTime.Now;
        }

        public Int32 GetCodeId(string type, string code)
        {
            Code c = GetCode(type, code);

            if (c != null) return c.CodeId;
            else return 0;
        }

        public Code GetCode(string type, string code)
        {
            if (_tList.ContainsKey(type))
            {
                foreach (Code c in _tList[type])
                {
                    if (String.Compare(code, c.CodeDefn, true) == 0)
                    {
                        return c;
                    }
                }
            }

            return null;
        }

        public Code GetCode(int codeId)
        {
            if (_iList.ContainsKey(codeId))
            {
                return _iList[codeId];
            }

            return null;
        }

        public Code GetCode(Guid codeGuid)
        {
            if (_gList.ContainsKey(codeGuid))
            {
                return _gList[codeGuid];
            }

            return null;
        }

        public IEnumerable<Code> GetCodes(string type)
        {
            if (_tList.ContainsKey(type))
            {
                return _tList[type];
            }

            return null;
        }

        public void UpdateCode(Code code)
        {
            if (_iList.ContainsKey(code.CodeId))
            {
                _iList[code.CodeId].Update(code);
            }
            else
            {
                _codes.Add(code);

                // Take a hit on insert for faster reads
                UpdateIList(code);
                UpdateGList(code);
                UpdateTList(code);
            }
        }

        public void AddCodes(List<Code> codes)
        {
            foreach (Code c in codes)
            {
                UpdateCode(c);
            }
        }

        private void UpdateIList(Code c)
        {
            if (!_iList.ContainsKey(c.CodeId))
                _iList.Add(c.CodeId, c);
        }

        private void UpdateTList(Code c)
        {
            if (_tList.ContainsKey(c.CodeTypeDefn))
            {
                if (!_tList[c.CodeTypeDefn].Contains(c))
                    _tList[c.CodeTypeDefn].Add(c);
            }
            else
            {
                _tList.Add(c.CodeTypeDefn, new CodeCollection { c });
            }
        }

        private void UpdateGList(Code c)
        {
            if (!_gList.ContainsKey(c.CodeGuid))
                _gList.Add(c.CodeGuid, c);
        }

        private void GetCodesFromMaster(string type)
        {
        }
    }
}
