using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moudou.DBStructureReader.AbstractionClasses
{
    /// <summary> 欄位資訊 </summary>
    public class ColumnInfo
    {
        /// <summary> 欄位名稱 </summary>
        public string Name { get; set; }

        /// <summary> 欄位型別 </summary>
        public string ColumnType { get; set; }

        /// <summary> 在 .NET 中對應的型別 </summary>
        public string DotNetType { get; set; } = "string";

        /// <summary> 是否為自動增號 </summary>
        public bool IsIdentity { get; set; }

        /// <summary> 是否為主鍵 </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary> 是否為唯一鍵 </summary>
        public bool IsUniqueKey { get; set; }

        /// <summary> 是否為外部鍵 </summary>
        public bool IsForignKey { get; set; }

        /// <summary> 長度 </summary>
        public int? Length { get; set; }

        /// <summary> 是否允許空值 </summary>
        public bool IsNullable { get; set; }
    }
}
