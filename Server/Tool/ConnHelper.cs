using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GameServer.Tool
{
    class ConnHelper
    {
        public const string CONNECTIONSTRING = "datasource=127.0.0.1;port=3306;database=game01;user=root;pwd=147896325aa.;";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch(Exception e)
            {
                return null;
            }
            
        }
        public static void CloseConnection(MySqlConnection conn)
        {
            if(conn!=null)
                conn.Close();
        }
    }
}
