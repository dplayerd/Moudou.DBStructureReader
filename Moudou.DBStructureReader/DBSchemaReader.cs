using Moudou.DBStructureReader.AbstractionClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moudou.DBStructureReader
{
    /// <summary> 資料庫結構讀取器 </summary>
    public class DBSchemaReader
    {
        /// <summary> 讀取結構 </summary>
        /// <param name="connectionNameOrString"></param>
        /// <returns></returns>
        public IEnumerable<TableInfo> GetSchema(string connectionNameOrString)
        {
            var reader = new SqlServerSchemaReader();
            DataTable dt = reader.ReadColumnSchemas(connectionNameOrString);

            var dic = new Dictionary<string, TableInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                var tableName = dr.Field<string>("TABLE_NAME");
                var columnName = dr.Field<string>("COLUMN_NAME");
                var isNullable = dr.Field<string>("IS_NULLABLE");
                var columnType = dr.Field<string>("DATA_TYPE");
                var textLength = dr.Field<int?>("CHARACTER_MAXIMUM_LENGTH");
                var numericPrecision = dr.Field<byte?>("NUMERIC_PRECISION");
                var numericScale = dr.Field<int?>("NUMERIC_SCALE");
                var columnDefault = dr.Field<string>("COLUMN_DEFAULT");

                if (!dic.ContainsKey(tableName))
                    dic.Add(tableName, new TableInfo(tableName));

                var col = new ColumnInfo()
                {
                    Name = columnName,
                    ColumnType = columnType,
                    Length = textLength,
                    IsNullable = (string.Compare("YES", isNullable, true) == 0)
                };

                dic[tableName].ColumnInfos.Add(col);
            }

            return dic.Values;
        }
    }
}
