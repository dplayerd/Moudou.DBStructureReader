using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moudou.DBStructureReader.AbstractionClasses
{
    /// <summary> 表格資訊 </summary>
    public class TableInfo
    {
        public TableInfo(string name)
        {
            this.Name = name;
            ColumnInfos = new List<ColumnInfo>();
        }

        /// <summary> 表格名稱 </summary>
        public string Name { get; set; }

        /// <summary> 表格描述 </summary>
        public string Desc { get; set; }

        /// <summary> 欄位集合 </summary>
        public List<ColumnInfo> ColumnInfos { get; set; }
    }
}
