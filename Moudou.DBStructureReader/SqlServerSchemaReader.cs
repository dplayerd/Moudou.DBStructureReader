using System.Data;
using System.Data.SqlClient;

namespace Moudou.DBStructureReader
{
    internal class SqlServerSchemaReader
    {
        /// <summary> 讀取欄位結構 </summary>
        /// <param name="connectionNameOrString"></param>
        /// <returns></returns>
        internal DataTable ReadColumnSchemas(string connectionNameOrString)
        {
            string connectionString = ConfigHelper.GetConnectionString(connectionNameOrString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //connection.Open();
                //DataTable table = connection.GetSchema("AllColumns");
                //return table;

                connection.Open();

                string query =
                    @"  SELECT 
	                        *,
	                        COLUMNPROPERTY(object_id(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS IsIdentity
                        FROM INFORMATION_SCHEMA.COLUMNS 
                    ";

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                return table;
            }
        }


        // 參考這裡 https://docs.microsoft.com/zh-tw/sql/t-sql/functions/objectproperty-transact-sql?view=sql-server-ver15
        /// <summary> 讀取所有 Key 欄位結構(PK / UK / FK) </summary>
        /// <param name="connectionNameOrString"></param>
        /// <returns></returns>
        internal DataTable ReadKeySchemas(string connectionNameOrString)
        {
            string connectionString = ConfigHelper.GetConnectionString(connectionNameOrString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query =
                    @"  SELECT
                            COLUMN_NAME,
                            TABLE_NAME,
                            ObjectProperty(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QuoteName(CONSTRAINT_NAME)), 'IsPrimaryKey') AS IsPK,
                            ObjectProperty(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QuoteName(CONSTRAINT_NAME)), 'IsForeignKey') AS IsFK,
                            ObjectProperty(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QuoteName(CONSTRAINT_NAME)), 'IsUniqueCnst') AS IsUK
                        FROM 
                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                    ";

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                return table;
            }
        }
    }
}
