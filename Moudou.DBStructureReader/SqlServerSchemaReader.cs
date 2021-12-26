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
                connection.Open();
                DataTable table = connection.GetSchema("AllColumns");
                return table;
            }
        }
    }
}
