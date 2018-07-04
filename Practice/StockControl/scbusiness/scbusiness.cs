using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace StockControl.Core
{
    public class scbusiness
    {
        Data.StockControlDTO _dto = Data.StockControlDTO.Instance;

        public scbusiness()
        {
            DataTable dt = _dto.GetCodes();
        }
    }
}
