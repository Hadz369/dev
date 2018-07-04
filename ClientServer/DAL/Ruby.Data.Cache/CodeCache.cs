using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ruby.Data
{
    public class CodeCache
    {
        DateTime _refreshed = new DateTime(1800, 1, 1);
        List<Code> _codes = new List<Code>();

        // Different dictionaries for faster searches
        SortedDictionary<int, Code> _iList  = new SortedDictionary<int, Code>();
        SortedDictionary<Guid, Code> _gList = new SortedDictionary<Guid, Code>(); 
        SortedDictionary<string, List<Code>> _tList = new SortedDictionary<string, List<Code>>();

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
        
        public List<Code> GetCodes(string type)
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
                UpdateIList (code);
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
                _tList.Add(c.CodeTypeDefn, new List<Code> { c } );
            }
        }

        private void UpdateGList(Code c)
        {
            if (!_gList.ContainsKey(c.CodeGuid))
                _gList.Add(c.CodeGuid, c);
        }
    }
}
