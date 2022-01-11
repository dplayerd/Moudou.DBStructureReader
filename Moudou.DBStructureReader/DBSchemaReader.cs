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
        private static string[] _nullableDotNetType = { "string", "byte[]" };

        private static Dictionary<string, string> _dicTypeMapping = new Dictionary<string, string>()
        {
            { "bigint", "long" },
            { "binary", "byte[]" },
            { "bit", "bool" },
            { "char", "string" },
            { "datetime", "DateTime" },
            { "decimal", "decimal" },
            { "float", "double" },
            { "image", "byte[]" },
            { "int", "int" },
            { "money", "decimal" },
            { "nchar", "string" },
            { "ntext", "string" },
            { "numeric", "decimal" },
            { "nvarchar", "string" },
            { "real", "single" },
            { "rowversion", "byte[]" },
            { "smalldatetime", "DateTime" },
            { "smallint", "short" },
            { "smallmoney", "decimal" },
            { "text", "string" },
            { "time", "TimeSpan" },
            { "timestamp", "byte[]" },
            { "tinyint", "byte" },
            { "uniqueidentifier", "Guid" },
            { "varbinary", "byte[]" },
            { "varchar", "string" },
            { "xml", "Xml" },
        };

        /// <summary> 讀取結構 </summary>
        /// <param name="connectionNameOrString"></param>
        /// <returns></returns>
        public IEnumerable<TableInfo> GetSchema(string connectionNameOrString)
        {
            var reader = new SqlServerSchemaReader();
            DataTable dtColumnSchema = reader.ReadColumnSchemas(connectionNameOrString);
            DataTable dtKeySchema = reader.ReadKeySchemas(connectionNameOrString);

            var dicKeySchema = dtKeySchema.AsEnumerable().Select(dr =>
                new
                {
                    TableName = dr["TABLE_NAME"] as string,
                    ColumnName = dr["COLUMN_NAME"] as string,
                    IsPK = (dr["IsPK"] as int? ?? 0) > 0,
                    IsUK = (dr["IsUK"] as int? ?? 0) > 0,
                    IsFK = (dr["IsFK"] as int? ?? 0) > 0
                }).ToDictionary(x => $"{x.TableName}_{x.ColumnName}".ToLower(), x => x);

            var dic = new Dictionary<string, TableInfo>();
            foreach (DataRow dr in dtColumnSchema.Rows)
            {
                var tableName = dr.Field<string>("TABLE_NAME");
                var columnName = dr.Field<string>("COLUMN_NAME");
                var isNullable = dr.Field<string>("IS_NULLABLE");
                var columnType = dr.Field<string>("DATA_TYPE");
                var textLength = dr.Field<int?>("CHARACTER_MAXIMUM_LENGTH");
                var numericPrecision = dr.Field<byte?>("NUMERIC_PRECISION");
                var numericScale = dr.Field<int?>("NUMERIC_SCALE") ?? 0;
                var columnDefault = dr.Field<string>("COLUMN_DEFAULT");
                var isIdentity = dr.Field<int>("IsIdentity") > 0;

                if (!dic.ContainsKey(tableName))
                    dic.Add(tableName, new TableInfo(tableName));

                var col = new ColumnInfo()
                {
                    Name = columnName,
                    ColumnType = columnType,
                    Length = textLength,
                    IsIdentity = isIdentity,
                    IsNullable = (string.Compare("YES", isNullable, true) == 0)
                };

                // 取得 .net 格式
                col.DotNetType = GetDotNetType(col);

                string tableColumnName = $"{tableName}_{columnName}".ToLower();
                if (dicKeySchema.ContainsKey(tableColumnName))
                {
                    var obj = dicKeySchema[tableColumnName];
                    col.IsUniqueKey = obj.IsUK;
                    col.IsPrimaryKey = obj.IsPK;
                    col.IsForignKey = obj.IsFK;
                }

                dic[tableName].ColumnInfos.Add(col);
            }

            return dic.Values;
        }

        /// <summary> 取得 .net 用欄位型別 </summary>
        /// <param name="col"> 資料庫型別 </param>
        /// <returns></returns>
        private static string GetDotNetType(ColumnInfo col)
        {
            string typeName = col.ColumnType.ToLower();
            if (!DBSchemaReader._dicTypeMapping.ContainsKey(typeName))
                return "string";

            var dotnetType = DBSchemaReader._dicTypeMapping[typeName];
            if (_nullableDotNetType.Contains(dotnetType))
            {
                return dotnetType;
            }
            else
            {
                if (col.IsNullable)
                    return dotnetType + "?";
                else
                    return dotnetType;
            }
        }
    }
}
