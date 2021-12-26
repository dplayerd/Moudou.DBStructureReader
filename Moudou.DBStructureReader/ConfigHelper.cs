using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moudou.DBStructureReader
{
    internal class ConfigHelper
    {
        /// <summary> 讀取指定的連線字串
        /// <para> 如果 Config 中沒有指定名稱，直接回傳輸入值 </para>
        /// </summary>
        /// <param name="connectionNameOrString"></param>
        /// <returns></returns>
        internal static string GetConnectionString(string connectionNameOrString = "DefaultConnection")
        {
            if (string.IsNullOrWhiteSpace(connectionNameOrString))
                connectionNameOrString = "DefaultConnection";

            var connObj = ConfigurationManager.ConnectionStrings[connectionNameOrString];
            if (connObj == null)
                return connectionNameOrString;
            else
                return connObj.ConnectionString;
        }
    }
}
