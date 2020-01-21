using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace KHJHLog.Helper
{
    /// <summary>
    /// 針對資料做處理的物件
    /// </summary>
    class DataHelper
    {
        static internal DataTable convertXmlToDataTable(String rawXml)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader($"<Body>{rawXml}</Body>"));
            return ds.Tables[0];
        }


    }
}
